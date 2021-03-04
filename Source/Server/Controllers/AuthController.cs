using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Refactorizando.Shared.Data.Models;
using Refactorizando.Shared.Data.Models.Auth;
using Refactorizando.Server.Extensions;
using Refactorizando.Server.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Mabar.Cross.Images.Client.Services;
using Mabar.Cross.Images.Client.Models;
using AutoMapper;
using Refactorizando.Shared.Data.Models.Dtos;

namespace Refactorizando.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> logger;
        private readonly UserManager<SystemUser> userManager;
        private readonly SignInManager<SystemUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IExecutionContext context;
        private readonly IMapper mapper;
        private readonly IImageGenerationService imageGenerationService;
        private readonly ApplicationDbContext dbContext;

        public AuthController(
                ILogger<AuthController> logger,
                UserManager<SystemUser> userManager,
                SignInManager<SystemUser> signInManager,
                IConfiguration configuration,
                IExecutionContext context,
                IMapper mapper,
                IImageGenerationService imageGenerationService,
                ApplicationDbContext dbContext)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
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
            this.dbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));
        }


        [HttpPost("signup")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] SignUpRequest model)
        {
            var user = new SystemUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var url = string.Empty; //TODO: default unknown image
                var profilePictureUrlRequest = await imageGenerationService
                    .GenerateProfilePicture(new ProfilePictureRequest(){
                        PictureType = ProfilePictureRequest.PictureTypes.CenteredOneInitial,
                        Text = model.Name.ToLower(),
                    });
                if (profilePictureUrlRequest.IsOkResponse())
                {
                    url = profilePictureUrlRequest.Value;
                }
                var userReturned = await dbContext.SystemUsers
                                    .Where(k => k.Id == user.Id)
                                    .FirstAsync();
                userReturned.ProfileUrl = url;
                await dbContext.SaveChangesAsync();
                return BuildToken(model.Email, user.Id, new List<string>());
            }
            else
            {
                var errorDescription = string.Join(", ", result.Errors.Select(k => $"({k.Code}) {k.Description}"));
                return BadRequest(errorDescription);
            }
        }

        [HttpGet("renovate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Renovate()
        {
            var userInfo = new UserInfo()
            {
                Email = HttpContext.User.Identity.Name
            };
            var usuario = await userManager.FindByEmailAsync(userInfo.Email);
            var roles = await userManager.GetRolesAsync(usuario);
            return BuildToken(userInfo.Email, usuario.Id, roles);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(userInfo.Email);
                //await dbContext.SaveChangesAsync();
                var roles = await userManager.GetRolesAsync(user);
                return BuildToken(userInfo.Email, user.Id, roles);
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }

        [HttpGet("users/current")]
        public  async Task<IActionResult> GetUserInfo ()
        {
            var userId = context.GetUserId();
            var user = await dbContext.SystemUsers.FirstOrDefaultAsync(k => k.Id == userId);
            if (user == null)
            {
                return NotFound($"Can't find user withd Id={userId}");
            }
            var mapped = mapper.Map<SystemUserDto>(user);
            return Ok(mapped);
        } 
        private UserToken BuildToken(string email, string userId, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, email),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
