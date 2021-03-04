using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refactorizando.Shared.Data.Models.Responses;

namespace Refactorizando.Client.Data.Services {

    public interface IHttpManagerService
    {

        Task<ObjectHttpResponse<TOut>> Post<TIn, TOut>(string url, TIn data);
        Task<HttpResponse> Post(string url);
        
        Task<HttpResponse> Post<TIn>(string url, TIn data);
        Task<HttpResponse> PostMultipartFormDataContent(string url, MultipartFormDataContent data);


        Task<ObjectHttpResponse<TOut>> Put<TIn, TOut>(string url, Guid resourceId, TIn data);

        Task<HttpResponse> Put<TIn>(string url, Guid resourceId, TIn data);
        Task<HttpResponse> Put<TIn>(string url,  TIn data);


        Task<ObjectHttpResponse<TOut>> Patch<TIn, TOut>(string url, TIn data);
        Task<HttpResponse> Patch<TIn>(string url, TIn data);

        Task<ObjectHttpResponse<string>> Get(string url);

        Task<ObjectHttpResponse<TOut>> Get<TOut>(string url);
        Task<ObjectHttpResponse<TOut>> Get<TOut>(string url, Dictionary<string, string> query);
        Task<ObjectHttpResponse<TOut>> Get<TOut>(string url, Guid resourceId);
        Task<ObjectHttpResponse<TOut>> Get<TOut>(string url, Guid resourceId, Dictionary<string, string> query);

        Task<HttpResponse> Delete<TOut>(string url, Dictionary<string, string> query);
        Task<HttpResponse> Delete<TOut>(string url);
        Task<HttpResponse> Delete<TIn>(string url, TIn data);
        Task<HttpResponse> Delete(string url, Dictionary<string, string> query);
        Task<HttpResponse> Delete(string url);
        Task<HttpResponse> Delete(string url, Guid resourceId);
        Task<HttpResponse> Delete(string url, Guid resourceId1, Guid resourceId2);
        void SetBearer(string token);
        void UnsetBearer();

        string GetServiceEndpoint();
    }
}