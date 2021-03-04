using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Refactorizando.Shared.Utilities;

namespace Refactorizando.Shared.Data.Models.Responses{
    public class HttpResponse
    {
        public Dictionary<string, string> Headers { get; set; }

        public HttpResponse(HttpResponseMessage httpResponseMessage)
        {
            HttpResponseMessage = httpResponseMessage;
            SetHeaders();

            if (HttpResponseMessage.IsSuccessStatusCode)
            {
                Ok();
            }
            else
            {
                Error();
            }
        }

        private void SetHeaders()
        {
            Headers = new Dictionary<string, string>();
            foreach (var item in HttpResponseMessage.Headers)
            {
                var key = item.Key;
                var value = item.Value.Any() ? item.Value.First() : null;
                Headers.Add(key, value);
            }
        }

        public HttpResponseMessage HttpResponseMessage { get; set; }

        public ServiceResponseType Type { get; set; } = ServiceResponseType.Undefined;
        public string Message { get; set; }
        public HttpStatusCode Code { get; set; }

        public T TryGetHeaderValue<T>(string headerName, T defaultValue)
        {
            if (Headers.ContainsKey(headerName))
            {
                try
                {
                    return ParseUtility.ChangeType<T>(Headers[headerName]);
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        public async Task<string> GetBody()
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }

        private HttpResponse SetStatus(
            ServiceResponseType type,
            HttpStatusCode httpStatusCode,
            string message = null)
        {
            Type = type;
            Code = httpStatusCode;
            Message = message;
            return this;
        }


        public HttpResponse Error()
        {
            return SetStatus(type: ServiceResponseType.Error,
                             httpStatusCode: HttpResponseMessage.StatusCode,
                             message: $"Request returned error [{HttpResponseMessage.StatusCode.ToString()} ({(int)HttpResponseMessage.StatusCode})] {GetBody().Result}");
        }

        public HttpResponse Ok()
        {
            return SetStatus(type: ServiceResponseType.Ok,
                             httpStatusCode: HttpResponseMessage.StatusCode);
        }

        public virtual bool IsOkResponse() =>
            Type == ServiceResponseType.Ok;
    }
}