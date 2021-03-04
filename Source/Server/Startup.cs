using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.AspNetCore.Identity;
using Refactorizando.Shared.Data.Models;
using Refactorizando.Server.Extensions;
using Refactorizando.Server.Services;
using Refactorizando.Server.Services.Implementations;
using AutoMapper;
using Mabar.Cross.Mailing.Client.Extensions;
using Mabar.Cross.Mailing.Client.Models;
using Mabar.Cross.Images.Client.Extensions;
using Mabar.Cross.Images.Client.Models;

namespace Refactorizando.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddHealthChecks();
            services.AddCors();
            services.AddDbContextPool<ApplicationDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        Configuration.GetConnectionString(),
                        new MySqlServerVersion(new Version(8, 0, 22)),
                        mySqlOptions => mySqlOptions
                    .EnableRetryOnFailure()
                    .CharSetBehavior(CharSetBehavior.NeverAppend))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration.GetJwt())),
                    ClockSkew = TimeSpan.Zero,
                
                });

            services.AddIdentity<SystemUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

            services.AddMailingService(new EmailServerConfiguration()
            {
                Endpoint = Configuration.GetEmailEndpoint(),
                Token = Configuration.GetEmailToken(),
                ServerId = Configuration.GetEmailServerId()
            });
            services.AddProfilePictureService(new ProfilePictureServiceConfiguration(){
                Endpoint = Configuration.GetImagesEndpoint(),
                Token = Configuration.GetImagesToken()
            });
            services.AddScoped<IExecutionContext, HttpContextExecutionContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
