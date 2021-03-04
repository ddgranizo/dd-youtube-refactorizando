using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Refactorizando.Shared.Data.Models.Responses;

namespace Refactorizando.Client.Data.Services.Implementations
{
    public class HttpManagerService : IHttpManagerService
    {

        public HttpManagerService(HttpClient client)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            this.client = client;
        }
        private string token;
        private readonly HttpClient client;

        private HttpClient GetClient(string uri)
        {
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
            return client;
        }
        public async Task<ObjectHttpResponse<TOut>> Patch<TIn, TOut>(string url, TIn data)
        {
            var content = GetStringContent(data);
            var responseHttp = await GetClient(url).PatchAsync(url, content);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        public async Task<HttpResponse> Patch<TIn>(string url, TIn data)
        {
            var content = GetStringContent(data);
            var responseHttp = await GetClient(url).PatchAsync(url, content);
            return new HttpResponse(responseHttp);
        }

        public async Task<ObjectHttpResponse<TOut>> Post<TIn, TOut>(string url, TIn data)
        {
            var content = GetStringContent(data);
            var responseHttp = await GetClient(url).PostAsync(url, content);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        public async Task<HttpResponse> PostMultipartFormDataContent(string url, MultipartFormDataContent data)
        {
            var responseHttp = await GetClient(url).PostAsync(url, data);
            return new HttpResponse(responseHttp);
        }


        public async Task<HttpResponse> Post(string url)
        {
            var responseHttp = await GetClient(url).PostAsync(url, null);
            return new HttpResponse(responseHttp);
        }

        public async Task<HttpResponse> Post<TIn>(string url, TIn data)
        {
            var content = GetStringContent(data);
            var responseHttp = await GetClient(url).PostAsync(url, content);
            return new HttpResponse(responseHttp);
        }

        public async Task<ObjectHttpResponse<TOut>> Put<TIn, TOut>(string url, Guid resourceId, TIn data)
        {
            var content = GetStringContent(data);
            var composedUrl = GetUrlForResource(url, resourceId);
            var responseHttp = await GetClient(url).PutAsync(composedUrl, content);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        public async Task<HttpResponse> Put<TIn>(string url, Guid resourceId, TIn data)
        {
            var content = GetStringContent(data);
            var composedUrl = GetUrlForResource(url, resourceId);
            var responseHttp = await GetClient(url).PutAsync(composedUrl, content);
            return new HttpResponse(responseHttp);
        }

         public async Task<HttpResponse> Put<TIn>(string url,  TIn data)
        {
            var content = GetStringContent(data);
            var composedUrl = url;
            var responseHttp = await GetClient(url).PutAsync(composedUrl, content);
            return new HttpResponse(responseHttp);
        }

        public async Task<ObjectHttpResponse<TOut>> Get<TOut>(string url, Dictionary<string, string> query)
        {
            var completedUrl = url;
            if (query.Count > 0)
            {
                completedUrl = $"{completedUrl}?{DictionaryToQueryParameters(query)}";
            }
            Console.WriteLine();
            var responseHttp = await GetClient(url).GetAsync(completedUrl);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        public async Task<ObjectHttpResponse<string>> Get(string url)
        {
            var responseHttp = await GetClient(url).GetAsync(url);
            return new ObjectHttpResponse<string>(responseHttp);
        }
        public async Task<ObjectHttpResponse<TOut>> Get<TOut>(string url)
        {
            var responseHttp = await GetClient(url).GetAsync(url);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        public async Task<ObjectHttpResponse<TOut>> Get<TOut>(string url, Guid resourceId, Dictionary<string, string> query)
        {
            var composedUrl = GetUrlForResource(url, resourceId);
            var completedUrl = composedUrl;
            if (query.Count > 0)
            {
                completedUrl = $"{completedUrl}?{DictionaryToQueryParameters(query)}";
            }
            var responseHttp = await GetClient(url).GetAsync(completedUrl);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        public async Task<ObjectHttpResponse<TOut>> Get<TOut>(string url, Guid resourceId)
        {
            var composedUrl = GetUrlForResource(url, resourceId);
            var responseHttp = await GetClient(url).GetAsync(composedUrl);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        public async Task<HttpResponse> Delete<TOut>(string url, Dictionary<string, string> query)
        {
            var completedUrl = url;
            if (query.Count > 0)
            {
                completedUrl = $"{completedUrl}?{DictionaryToQueryParameters(query)}";
            }
            var responseHttp = await GetClient(url).DeleteAsync(completedUrl);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        public async Task<HttpResponse> Delete<TOut>(string url)
        {
            var responseHttp = await GetClient(url).DeleteAsync(url);
            return new ObjectHttpResponse<TOut>(responseHttp);
        }

        
        public async Task<HttpResponse> Delete(string url, Dictionary<string, string> query)
        {
            var completedUrl = url;
            if (query.Count > 0)
            {
                completedUrl = $"{completedUrl}?{DictionaryToQueryParameters(query)}";
            }
            var responseHttp = await GetClient(url).DeleteAsync(completedUrl);
            return new HttpResponse(responseHttp);
        }

        public async Task<HttpResponse> Delete(string url)
        {
            var responseHttp = await GetClient(url).DeleteAsync(url);
            return new HttpResponse(responseHttp);
        }

        public async Task<HttpResponse> Delete(string url, Guid resourceId)
        {
            var composedUrl = GetUrlForResource(url, resourceId);
            var responseHttp = await GetClient(url).DeleteAsync(composedUrl);
            return new HttpResponse(responseHttp);
        }

        public async Task<HttpResponse> Delete(string url, Guid resourceId1, Guid resourceId2)
        {
            var composedUrl = GetUrlForResource(url, resourceId1, resourceId2);
            var responseHttp = await GetClient(url).DeleteAsync(composedUrl);
            return new HttpResponse(responseHttp);
        }

        public async Task<HttpResponse> Delete<TIn>(string url, TIn data)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(client.BaseAddress, url),
            };
            var responseHttp = await client.SendAsync(request);
            return new HttpResponse(responseHttp);
        }

        private static string GetUrlForResource(string url, Guid resourceId)
        {
            var composedUrl = url;
            if (composedUrl.Last() != '/')
            {
                composedUrl = $"{composedUrl}/";
            }
            composedUrl = $"{composedUrl}{resourceId.ToString()}";
            return composedUrl;
        }

        private static string GetUrlForResource(string url, Guid resourceId1, Guid resourceId2)
        {
            return GetUrlForResource(GetUrlForResource(url, resourceId1), resourceId2);
        }

        private static string DictionaryToQueryParameters(Dictionary<string, string> query)
        {
            return string.Join("&", query
                .Select(k => $"{k.Key}={HttpUtility.UrlEncode(k.Value, Encoding.UTF8)}")
                .ToArray());
        }

        private static StringContent GetStringContent<TIn>(TIn data)
        {
            var body = JsonSerializer.Serialize(data);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            return content;
        }

        public void SetBearer(string token)
        {
            this.token = token;
        }

        public void UnsetBearer()
        {
            this.token = null;
        }

        public string GetServiceEndpoint()
        {
            return client.BaseAddress.ToString();
        }
    }
}