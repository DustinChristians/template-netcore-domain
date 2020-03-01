using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
using CompanyName.ProjectName.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyName.ProjectName.WebApi.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> logger;
        private readonly IMapper mapper;
        private readonly IMessagesService messagesService;

        public MessagesController(ILogger<MessagesController> logger, IMapper mapper, IMessagesService messagesService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.messagesService = messagesService;
        }

        [HttpGet, HttpHead]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages([FromQuery] MessagesResourceParameters parameters)
        {
            var result = await messagesService.MessagesRepository.GetMessagesAsync(parameters);

            return result == null ? NotFound() : (ActionResult)Ok(mapper.Map<IEnumerable<Message>>(result));
        }

        [HttpGet("{messageId:int}")]
        public async Task<ActionResult<Message>> GetMessage(int messageId)
        {
            var result = await messagesService.MessagesRepository.GetByIdAsync(messageId);

            return result == null ? NotFound() : (ActionResult)Ok(result);
        }

        [HttpGet("{messageId:guid}")]
        public async Task<ActionResult<Message>> GetMessage(Guid messageId)
        {
            var result = await messagesService.MessagesRepository.GetByGuidAsync(messageId);

            return result == null ? NotFound() : (ActionResult)Ok(result);
        }
    }
}
