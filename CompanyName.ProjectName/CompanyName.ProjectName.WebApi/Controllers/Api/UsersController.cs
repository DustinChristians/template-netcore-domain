using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
using CompanyName.ProjectName.WebApi.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyName.ProjectName.WebApi.Controllers
{
    [ApiController]
    public class UsersController : BaseController<UsersController>
    {
        private readonly IUsersService usersService;

        public UsersController(
            IUsersService usersService,
            ILogger<UsersController> logger,
            IMapper mapper)
            : base(logger, mapper)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<ReadUser>>> GetUsers([FromQuery] UsersResourceParameters parameters)
        {
            var users = await usersService.UsersRepository.GetUsersAsync(parameters);

            return users == null ? NotFound() : (ActionResult)Ok(mapper.Map<IEnumerable<ReadUser>>(users));
        }

        [HttpGet("{userId:int}", Name = "GetUserById")]
        public async Task<ActionResult<ReadUser>> GetUser(int userId)
        {
            var user = await usersService.UsersRepository.GetByIdAsync(userId);

            return user == null ? NotFound() : (ActionResult)Ok(mapper.Map<ReadUser>(user));
        }

        [HttpGet("{userId:guid}", Name = "GetUserByGuid")]
        public async Task<ActionResult<ReadUser>> GetUser(Guid userId)
        {
            var user = await usersService.UsersRepository.GetByGuidAsync(userId);

            return user == null ? NotFound() : (ActionResult)Ok(mapper.Map<ReadUser>(user));
        }

        [HttpPost]
        public async Task<ActionResult<ReadUser>> PostUser(CreateUser createUser)
        {
            var user = mapper.Map<User>(createUser);

            await usersService.UsersRepository.CreateAsync(user);

            var result = mapper.Map<ReadUser>(user);

            return CreatedAtRoute("GetUserByGuid", new { userId = result.Guid }, result);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var user = await usersService.UsersRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            usersService.UsersRepository.DeleteAsync(user);
            await usersService.UsersRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "DELETE, GET, HEAD, OPTIONS, POST");

            return Ok();
        }
    }
}
