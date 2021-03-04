using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Mabar.Cross.Mailing.Client.Models;
using Mabar.Cross.Mailing.Client.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Refactorizando.Server.Services;
using Refactorizando.Shared.Data.Models;
using Refactorizando.Shared.Data.Models.Dtos;

namespace Refactorizando.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LikesController: Controller
    {
        private readonly ILogger<LikesController> logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IExecutionContext context;
        private readonly IMapper mapper;
        private readonly IMailingService mailingService;

        public LikesController(
            ILogger<LikesController> logger,
            ApplicationDbContext dbContext,
            IExecutionContext context,
            IMapper mapper,
            IMailingService mailingService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        

        [HttpPost("{requestId}")]
        public async Task<IActionResult> Like(Guid requestId)
        {
            var userId = context.GetUserId();
            var currentLike = await dbContext.LikeRequests
                .Where(k => k.SystemUserId == userId 
                            && k.RequestId == requestId)
                .FirstOrDefaultAsync();
            if (currentLike != null)
            {
                return BadRequest();
            }
            var mapped = new LikeRequest(){
                RequestId = requestId,
                SystemUserId = userId
            };
            mapped.SystemUserId = userId;
            var request = await dbContext.LikeRequests.AddAsync(mapped);
            await dbContext.SaveChangesAsync();
            return Ok(mapped.Id);
        }

        [HttpDelete("{requestId}")]
        public async Task<IActionResult> Dislike(Guid requestId)
        {
            var userId = context.GetUserId();
            var currentLike = await dbContext.LikeRequests
                .Where(k => k.SystemUserId == userId 
                            && k.RequestId == requestId)
                .SingleOrDefaultAsync();

            if (currentLike != null)
            {
                dbContext.LikeRequests.Remove(currentLike);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }
    }
}