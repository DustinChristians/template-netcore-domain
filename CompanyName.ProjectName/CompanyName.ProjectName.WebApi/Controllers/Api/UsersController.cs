using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
using CompanyName.ProjectName.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryUser = CompanyName.ProjectName.Core.Models.Repositories.User;

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

        [HttpGet, HttpHead]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] UsersResourceParameters parameters)
        {
            var result = await usersService.UsersRepository.GetUsersAsync(parameters);

            return result == null ? NotFound() : (ActionResult)Ok(mapper.Map<IEnumerable<User>>(result));
        }

        [HttpGet("{userId:int}", Name = "GetUserById")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var result = await usersService.UsersRepository.GetByIdAsync(userId);

            return result == null ? NotFound() : (ActionResult)Ok(result);
        }

        [HttpGet("{userId:guid}", Name = "GetUserByGuid")]
        public async Task<ActionResult<User>> GetUser(Guid userId)
        {
            var result = await usersService.UsersRepository.GetByGuidAsync(userId);

            return result == null ? NotFound() : (ActionResult)Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserForCreation user)
        {
            var entity = mapper.Map<RepositoryUser>(user);
            await usersService.UsersRepository.AddAsync(entity);
            await usersService.UsersRepository.SaveChangesAsync();

            var result = mapper.Map<User>(entity);
            return CreatedAtRoute("GetUserById", new { userId = result.Id }, result);
        }
    }
}
