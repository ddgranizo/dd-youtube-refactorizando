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
    public class RequestsController: Controller
    {
        private readonly ILogger<RequestsController> logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IExecutionContext context;
        private readonly IMapper mapper;
        private readonly IMailingService mailingService;

        public RequestsController(
            ILogger<RequestsController> logger,
            ApplicationDbContext dbContext,
            IExecutionContext context,
            IMapper mapper,
            IMailingService mailingService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.mailingService = mailingService 
                ?? throw new ArgumentNullException(nameof(mailingService));
                
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery]QueryParameters parameters)
        {
            var userId = context.GetUserId();
            var query =  dbContext.Requests
                .Skip((parameters.Page - 1)*parameters.Count)
                .Take(parameters.Count)
                .OrderByDescending(k => k.CreatedOn);

            var items = await query
                .Include(k => k.LikeRequests)
                .Include(k => k.SystemUser)
                .ToListAsync();
            var totalItemCount = query.Count();
            var totalPages = totalItemCount% parameters.Count + 1;
            var responseItems = new List<RequestDto>();
            foreach (var item in items)
            {
                var mapped = mapper.Map<RequestDto>(item);
                mapped.SystemUser.Email = null;
                if (!string.IsNullOrEmpty(userId) 
                        && item.LikeRequests != null)
                {
                    mapped.HasCurrentUserLike = 
                                    item.LikeRequests
                                    .Any(k => k.SystemUserId == userId);
                    mapped.LikesCount = item.LikeRequests.Count;
                }
                responseItems.Add(mapped);
            }
            var response = new DataSetResponse<RequestDto>(parameters){
                Values = responseItems,
                TotalCount = totalItemCount,
                TotalPages = totalPages,
            };
            return Ok(response);
        }
        
        [HttpGet("mine")]
        public async Task<IActionResult> GetMine([FromQuery]QueryParameters parameters)
        {
            var userId = context.GetUserId();
            var query =  dbContext.Requests
                .Skip((parameters.Page-1)*parameters.Count)
                .Take(parameters.Count)
                .OrderByDescending(k => k.CreatedOn)
                .Where(k => k.SystemUserId == userId);

            var items = await query
                .Include(k => k.LikeRequests)
                .ToListAsync();
            var totalItemCount = query.Count();
            var totalPages = totalItemCount% parameters.Count + 1;
            var responseItems = new List<RequestDto>();
            foreach (var item in items)
            {
                var mapped = mapper.Map<RequestDto>(item);
                if (item.LikeRequests != null)
                {
                    mapped.HasCurrentUserLike = item.LikeRequests.Any(k => k.SystemUserId == userId);
                    mapped.LikesCount = item.LikeRequests.Count;
                }
                responseItems.Add(mapped);
            }
            var response = new DataSetResponse<RequestDto>(parameters){
                Values = responseItems,
                TotalCount = totalItemCount,
                TotalPages = totalPages,
            };
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RequestDto data)
        {
            var userId = context.GetUserId();
            var record = await dbContext.Requests.FirstOrDefaultAsync(k => k.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            if (record.SystemUserId != userId)
            {
                return Unauthorized();
            }
            record.RepositoryUri = data.RepositoryUri;
            record.Description = data.Description;
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(RequestDto data)
        {
            var userId = context.GetUserId();
            data.Id = Guid.Empty;
            data.CreatedOn = DateTime.Now;
            data.SystemUserId = userId;
            data.State = Shared.Data.Models.Request.RequestStates.Pending;
            data.StateReason = Shared.Data.Models.Request.RequestStateReasons.Pending;
            var mapped = mapper.Map<Request>(data);
            var request = await dbContext.Requests.AddAsync(mapped);
            await dbContext.SaveChangesAsync();
            return Ok(mapped.Id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userId = context.GetUserId();

            var request = await dbContext.Requests
                .Where(k => k.Id == id)
                .FirstOrDefaultAsync();
            if (request == null)
            {
                return NotFound();
            }
            var mapped = mapper.Map<RequestDto>(request);
            if (request.LikeRequests != null)
            {
                mapped.LikesCount =  request.LikeRequests.Count ;
                mapped.HasCurrentUserLike = request.LikeRequests.Any(k => k.SystemUserId == userId);
            }
            return Ok(mapped);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = context.GetUserId();
            var record = await dbContext.Requests.FirstOrDefaultAsync(k => k.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            if (record.SystemUserId != userId)
            {
                return Unauthorized();
            }
            dbContext.Requests.Remove(record);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}