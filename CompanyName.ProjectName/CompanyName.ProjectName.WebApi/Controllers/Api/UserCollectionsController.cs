using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.WebApi.Models.User;
using CompanyName.ProjectName.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyName.ProjectName.WebApi.Controllers.Api
{
    [ApiController]
    [Route("api/usercollections")]
    public class UserCollectionsController : BaseController<UserCollectionsController>
    {
        private readonly IUsersService usersService;

        public UserCollectionsController(
            IUsersService usersService,
            ILogger<UserCollectionsController> logger,
            IMapper mapper)
            : base(logger, mapper)
        {
            this.usersService = usersService;
        }

        [HttpGet("({guids})", Name = "GetUserCollection")]
        public async Task<IActionResult> GetUserCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinderUtility))] IEnumerable<Guid> guids)
        {
            if (guids == null)
            {
                return BadRequest();
            }

            var users = await usersService.UsersRepository.GetByGuidsAsync(guids);

            if (guids.Count() != users.Count())
            {
                return NotFound();
            }

            var readUsers = mapper.Map<IEnumerable<ReadUser>>(users);

            return Ok(readUsers);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReadUser>>> CreateUserCollection(
            IEnumerable<CreateUser> createUsers)
        {
            var users = mapper.Map<List<User>>(createUsers);

            foreach (var user in users)
            {
                await usersService.UsersRepository.CreateAsync(user);
            }

            var createUsersToReturn = mapper.Map<IEnumerable<ReadUser>>(users);
            var guidsAsString = string.Join(",", createUsersToReturn.Select(x => x.Guid));

            return CreatedAtRoute(
                "GetUserCollection",
                new { guids = guidsAsString },
                createUsersToReturn);
        }

        [HttpOptions]
        public IActionResult GetUserCollectionsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}