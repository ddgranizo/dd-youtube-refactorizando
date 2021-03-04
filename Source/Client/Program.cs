using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refactorizando.Client.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Refactorizando.Client.Data.Services;
using Refactorizando.Client.Data.Services.Implementations;

namespace Refactorizando.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress =   new Uri(builder.HostEnvironment.BaseAddress) });
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync(); 
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorizationCore();

            services.AddScoped<AppAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>(
                provider => provider.GetRequiredService<AppAuthenticationStateProvider>());
            services.AddScoped<ILoginService, AppAuthenticationStateProvider>(
                provider => provider.GetRequiredService<AppAuthenticationStateProvider>());

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHttpManagerService, HttpManagerService>();
            services.AddScoped<ILocalStorageService, LocalStorageService>();

            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<ILikeRequestsService, LikeRequestsService>();

        }
    }
}
