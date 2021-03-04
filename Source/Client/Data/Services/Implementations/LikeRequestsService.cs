using System;
using System.Threading.Tasks;
using Refactorizando.Shared.Data.Models.Responses;

namespace Refactorizando.Client.Data.Services.Implementations
{
    public class LikeRequestsService : ILikeRequestsService
    {
        private readonly IHttpManagerService httpManagerService;
        private const string ApiPath = "api/likes";

        public LikeRequestsService(IHttpManagerService httpManagerService)
        {
            if (httpManagerService is null)
            {
                throw new ArgumentNullException(nameof(httpManagerService));
            }

            this.httpManagerService = httpManagerService;
        }
        public async Task<HttpResponse> Dislike(Guid requestId)
        {
            return await httpManagerService.Delete($"{ApiPath}/{requestId}");
        }

        public async Task<HttpResponse> Like(Guid requestId)
        {
            return await httpManagerService.Post($"{ApiPath}/{requestId}");
        }
    }
}