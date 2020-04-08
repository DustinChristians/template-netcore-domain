using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.WebApi.Models;
using CompanyName.ProjectName.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryUser = CompanyName.ProjectName.Core.Models.Repositories.User;

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

        [HttpGet("({ids})", Name = "GetUserCollection")]
        public async Task<IActionResult> GetUserCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var userEntities = await usersService.UsersRepository.GetByIdsAsync(ids);

            if (ids.Count() != userEntities.Count())
            {
                return NotFound();
            }

            var users = mapper.Map<IEnumerable<User>>(userEntities);

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> CreateUserCollection(
            IEnumerable<UserForCreation> userCollection)
        {
            var userEntities = mapper.Map<List<RepositoryUser>>(userCollection);

            foreach (var entity in userEntities)
            {
                await usersService.UsersRepository.AddAsync(entity);
            }

            await usersService.UsersRepository.SaveChangesAsync();

            var userCollectionToReturn = mapper.Map<IEnumerable<User>>(userEntities);
            var idsAsString = string.Join(",", userCollectionToReturn.Select(x => x.Id));

            return CreatedAtRoute("GetUserCollection",
             new { ids = idsAsString },
             userCollectionToReturn);
        }
    }
}

