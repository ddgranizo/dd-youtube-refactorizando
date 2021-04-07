using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Refactorizando.Server.Services.Implementations
{
    public class HttpContextExecutionContext : IExecutionContext
    {
        public HttpContextExecutionContext(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public IHttpContextAccessor HttpContextAccessor { get; }


        public string GetUserId()
        {
            return HttpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public List<string> GetUserRoles()
        {
            return HttpContextAccessor.HttpContext.User.Claims.Where(k => k.Type == ClaimTypes.Role).Select(k => k.Value).ToList();
        }

        public bool UserHasRole(string role){
            var roles = GetUserRoles();
            return roles.Any(k => k == role);
        }
    }
}

