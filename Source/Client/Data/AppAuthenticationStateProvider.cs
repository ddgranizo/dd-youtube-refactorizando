using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Refactorizando.Client.Data.Services;
using Refactorizando.Shared.Data.Models.Auth;

namespace Refactorizando.Client.Data
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider, ILoginService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IHttpManagerService httpManagerService;
        private readonly IAuthService authService;
        public static readonly string TokenLocalStorageKey = "auth.token";
        public static readonly string ExpirationTokenLocalStorageKey = "auth.expiration";
        public AppAuthenticationStateProvider(
            ILocalStorageService localStorageService, 
            IHttpManagerService httpManagerService,
            IAuthService authService)
        {
            this.localStorageService = localStorageService 
                ?? throw new System.ArgumentNullException(nameof(localStorageService));
            this.httpManagerService = httpManagerService 
                ?? throw new System.ArgumentNullException(nameof(httpManagerService));
            this.authService = authService 
                ?? throw new System.ArgumentNullException(nameof(authService));
        }

        private AuthenticationState Anonymous => 
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public override  async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorageService.GetFromLocalStorage(TokenLocalStorageKey);
            if (string.IsNullOrEmpty(token))
            {
                return Anonymous;
            }

            var expirationString = await localStorageService.GetFromLocalStorage(ExpirationTokenLocalStorageKey);
            DateTime expiration;
            if (DateTime.TryParse(expirationString, out expiration))
            {
                if (IsExpiredToken(expiration))
                {
                    await Clean();
                    return Anonymous;
                }
                if (ShouldRenovateToken(expiration))
                {
                    token = await GetRenovationToken(token);
                }
            }
            return SetDefaultBearer(token);
        }

        private async Task<string> GetRenovationToken(string token)
        {   
            httpManagerService.SetBearer(token);
            var newTokenResponse = await authService.Renovate();
            if (newTokenResponse.IsOkResponse())
            {
                var newToken = newTokenResponse.Value;
                await localStorageService.SetInLocalStorage(TokenLocalStorageKey, newToken.Token);
                await localStorageService.SetInLocalStorage(ExpirationTokenLocalStorageKey, newToken.Expiration.ToString());
                return newToken.Token;
            }
            return null;
        }

        private bool ShouldRenovateToken(DateTime expiration)
        {
            return expiration.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(5);
        }

        private bool IsExpiredToken(DateTime expiration)
        {
            return expiration <= DateTime.UtcNow;
        }

        public AuthenticationState SetDefaultBearer(string token)
        {
            httpManagerService.SetBearer(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }
        
        public async Task Login(UserToken token)
        {
            await localStorageService.SetInLocalStorage(TokenLocalStorageKey, token.Token);
            await localStorageService.SetInLocalStorage(ExpirationTokenLocalStorageKey, token.Expiration.ToString());
            var authState = SetDefaultBearer(token.Token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await Clean();
            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
        }

        private async Task Clean()
        {
            await localStorageService.RemoveLocalStorage(TokenLocalStorageKey);
            await localStorageService.RemoveLocalStorage(ExpirationTokenLocalStorageKey);
            httpManagerService.UnsetBearer();
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);
            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            }
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}