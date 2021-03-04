using System;
using System.Net.Http;
using System.Text.Json;

namespace Refactorizando.Shared.Data.Models.Responses {

    public class ObjectHttpResponse<TObject> : HttpResponse
    {
        public static JsonSerializerOptions defaultSerializeOptions => new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public ObjectHttpResponse(HttpResponseMessage httpResponseMessage)
            : base(httpResponseMessage)
        {
            if (IsOkResponse())
            {
                if (typeof(TObject) == typeof(string))
                {
                    Value = (TObject)(object)GetBody().Result;
                }
                else
                {
                    var body = GetBody().Result;
                    if (!string.IsNullOrEmpty( body) && !string.IsNullOrEmpty(body.Trim()))
                    {
                     Value = JsonSerializer.Deserialize<TObject>(GetBody().Result, defaultSerializeOptions);
                    }
                }
            }
        }
        public TObject Value { get; private set; }

    }
}