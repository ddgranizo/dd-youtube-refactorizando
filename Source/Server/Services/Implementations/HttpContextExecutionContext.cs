using System;
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
            return HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}

