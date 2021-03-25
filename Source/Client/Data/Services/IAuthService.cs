using System.Threading.Tasks;
using Refactorizando.Shared.Data.Models.Auth;
using Refactorizando.Shared.Data.Models.Dtos;
using Refactorizando.Shared.Data.Models.Responses;

namespace Refactorizando.Client.Data.Services
{
    public interface IAuthService
    {
        Task<ObjectHttpResponse<UserToken>> Login( UserInfo user); 
        Task<ObjectHttpResponse<UserToken>> SignUp( SignUpRequest request); 

        Task<ObjectHttpResponse<UserToken>> Renovate();
        // Task<ObjectHttpResponse<SystemUserDto>> GetUserInformation();
        Task<HttpResponse> ValideEmail(string userId, string token); 

    }
}