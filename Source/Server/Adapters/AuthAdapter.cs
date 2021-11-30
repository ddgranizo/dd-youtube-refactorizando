using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Mabar.Cross.Images.Client.Models;
using Mabar.Cross.Images.Client.Services;
using Mabar.Cross.Mailing.Client.Models;
using Mabar.Cross.Mailing.Client.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Refactorizando.Server.Extensions;
using Refactorizando.Server.Services;
using Refactorizando.Shared.Data.Models;
using Refactorizando.Shared.Data.Models.Auth;

namespace Refactorizando.Server.Adapters
{
    public class AuthAdapter
    {
        private readonly UserManager<SystemUser> userManager;
        private readonly SignInManager<SystemUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IExecutionContext context;
        private readonly IMapper mapper;
        private readonly IImageGenerationService imageGenerationService;
        private readonly IMailingService mailingService;
        private readonly ApplicationDbContext dbContext;
        public AuthAdapter(
                UserManager<SystemUser> userManager,
                SignInManager<SystemUser> signInManager,
                IConfiguration configuration,
                IExecutionContext context,
                IMapper mapper,
                IImageGenerationService imageGenerationService,
                IMailingService mailingService,
                ApplicationDbContext dbContext )
        {
           
            
            this.userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager
                ?? throw new ArgumentNullException(nameof(signInManager));
            this.configuration = configuration
                ?? throw new ArgumentNullException(nameof(configuration));
            this.context = context 
                ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper 
                ?? throw new ArgumentNullException(nameof(mapper));
            this.imageGenerationService = imageGenerationService 
                ?? throw new ArgumentNullException(nameof(imageGenerationService));
            this.mailingService = mailingService 
                ?? throw new ArgumentNullException(nameof(mailingService));
            this.dbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));
        }   


        public async Task<UserToken> CreateUser(SignUpRequest model){
            var user = new SystemUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var url = string.Empty; 
                var profilePictureUrlRequest = await imageGenerationService
                    .GenerateProfilePicture(new ProfilePictureRequest(){
                        PictureType = ProfilePictureRequest.PictureTypes.CenteredOneInitial,
                        Text = model.Name.ToUpper(),
                    });
                if (profilePictureUrlRequest.IsOkResponse())
                {
                    url = profilePictureUrlRequest.Value;
                }
                user.ProfileUrl = url;
                await userManager.AddToRoleAsync(user, "user");
                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var appEndpoint = configuration["APP_ENDPOINT"];
                string codeHtmlVersion = HttpUtility.UrlEncode(confirmationToken);
                var emailTemplateValues = new Dictionary<string, object>(){
                    {"name", user.Name}, 
                    {"serviceName", "Refactor tool"}, 
                    {"url", $"{appEndpoint}/email/validation?token={codeHtmlVersion}&id={user.Id}"}};
                await mailingService.SendEmailTemplate(new Email(){
                    Subject = "[Refactoring] Email confirmation ✉️",
                    To = new List<EmailAddress>(){
                        new EmailAddress(){
                            Email = user.Email,
                            Name = user.Name
                        }
                    }
                }, 
                new Guid(Definitions.ConfirmationEmailTemplateId),
                emailTemplateValues);
                await dbContext.SaveChangesAsync();
                return BuildToken(model.Email, user.Id, url, user.Name, new List<string>());
            }
            else
            {

            }
        }

        private UserToken BuildToken(string email, string userId, string profileImage, string userName, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, email),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("ProfileImage", profileImage),
                new Claim("UserName", userName),

            };
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            var jwt = configuration.GetJwt();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(30);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

    }
}