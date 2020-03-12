using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyName.ProjectName.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> logger;
        protected readonly IMapper mapper;

        public BaseController(ILogger<T> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }
    }
}
