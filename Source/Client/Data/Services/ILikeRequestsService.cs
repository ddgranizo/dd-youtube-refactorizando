using System;
using System.Threading.Tasks;
using Refactorizando.Shared.Data.Models.Responses;

namespace Refactorizando.Client.Data.Services
{
    public interface ILikeRequestsService
    {
        Task<HttpResponse> Like(Guid requestId);
        Task<HttpResponse> Dislike(Guid requestId);
    }
}