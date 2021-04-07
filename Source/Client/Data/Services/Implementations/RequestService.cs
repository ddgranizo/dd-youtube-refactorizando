using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refactorizando.Client.Data.Services;
using Refactorizando.Shared.Data.Models;
using Refactorizando.Shared.Data.Models.Dtos;
using Refactorizando.Shared.Data.Models.Responses;

namespace Refactorizando.Client.Data.Services.Implementations
{
    public class RequestService : IRequestService
    {
        private readonly IHttpManagerService httpManagerService;
        private const string ApiPath = "api/requests";

        public RequestService(IHttpManagerService httpManagerService)
        {
            this.httpManagerService = httpManagerService
                ?? throw new ArgumentNullException(nameof(httpManagerService));
        }

        public async Task<ObjectHttpResponse<DataSetResponse<RequestDto>>> GetMines(QueryParameters parameters)
        {
            return await httpManagerService.Get<DataSetResponse<RequestDto>>($"{ApiPath}/mine?count={parameters.Count}&page={parameters.Page}" );
        }

        public async Task<ObjectHttpResponse<DataSetResponse<RequestDto>>> GetAll(QueryParameters parameters)
        {
            return await httpManagerService.Get<DataSetResponse<RequestDto>>($"{ApiPath}?count={parameters.Count}&page={parameters.Page}");
        }

        public async Task<ObjectHttpResponse<RequestDto>> Get(Guid id)
        {
            return await httpManagerService.Get<RequestDto>($"{ApiPath}/{id}");
        }

        public async Task<ObjectHttpResponse<Guid>> Create(RequestDto request)
        {
            return await httpManagerService.Post<RequestDto, Guid>($"{ApiPath}", request);
        }

        public async Task<HttpResponse> Update(Guid id, RequestDto request)
        {
            return await httpManagerService.Put($"{ApiPath}/{id}", request);
        }

        public async Task<HttpResponse> UpdateState(Guid id, int state)
        {
            return await httpManagerService.Put($"{ApiPath}/{id}/state/{state}", new{});
        }

        public async Task<HttpResponse> UpdateStateReason(Guid id, int stateReason)
        {
            return await httpManagerService.Put($"{ApiPath}/{id}/statereason/{stateReason}", new{});
        }

        public async Task<HttpResponse> Delete(Guid id)
        {
            return await httpManagerService.Delete($"{ApiPath}/{id}");
        }
    }
}
