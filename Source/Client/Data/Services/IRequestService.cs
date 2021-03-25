using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refactorizando.Shared.Data.Models;
using Refactorizando.Shared.Data.Models.Dtos;
using Refactorizando.Shared.Data.Models.Responses;

namespace Refactorizando.Client.Data.Services {
    public interface IRequestService
    {
        Task<ObjectHttpResponse<DataSetResponse<RequestDto>>> GetAll(QueryParameters parameters);
        Task<ObjectHttpResponse<DataSetResponse<RequestDto>>> GetMines(QueryParameters parameters);
        Task<ObjectHttpResponse<RequestDto>> Get(Guid id);
        Task<ObjectHttpResponse<Guid>> Create(RequestDto request);
        Task<HttpResponse> Update(Guid id, RequestDto request);
        Task<HttpResponse> Delete(Guid id);
    }
}

