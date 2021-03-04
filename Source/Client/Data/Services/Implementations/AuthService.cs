using System;
using System.Threading.Tasks;
using Refactorizando.Shared.Data.Models.Auth;
using Refactorizando.Shared.Data.Models.Dtos;
using Refactorizando.Shared.Data.Models.Responses;

namespace Refactorizando.Client.Data.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IHttpManagerService httpManagerService;
        private const string ApiPath = "api/auth";        
        public AuthService(IHttpManagerService httpManagerService)
        {
            this.httpManagerService = httpManagerService 
                ?? throw new System.ArgumentNullException(nameof(httpManagerService));
        }
        public  async Task<ObjectHttpResponse<UserToken>> Login( UserInfo user)
        {
            return await httpManagerService.Post<UserInfo, UserToken>($"{ApiPath}/login", user);
        }

        public async Task<ObjectHttpResponse<UserToken>> Renovate()
        {
            return await httpManagerService.Get<UserToken>($"{ApiPath}/renovate");
        }

        public async Task<ObjectHttpResponse<UserToken>> SignUp(SignUpRequest request)
        {
            return await httpManagerService.Post<SignUpRequest, UserToken>($"{ApiPath}/signup", request);
        }

         public async Task<ObjectHttpResponse<SystemUserDto>> GetUserInformation()
        {
            return await httpManagerService.Get<SystemUserDto>($"{ApiPath}/users/current");
        }
    }
}