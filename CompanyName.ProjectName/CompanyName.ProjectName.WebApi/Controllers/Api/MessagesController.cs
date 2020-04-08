using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
using CompanyName.ProjectName.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryMessage = CompanyName.ProjectName.Core.Models.Repositories.Message;

namespace CompanyName.ProjectName.WebApi.Controllers
{
    [ApiController]
    public class MessagesController : BaseController<MessagesController>
    {
        private readonly IMessagesService messagesService;

        public MessagesController(
            IMessagesService messagesService,
            ILogger<MessagesController> logger,
            IMapper mapper)
            : base(logger, mapper)
        {
            this.messagesService = messagesService;
        }

        [HttpGet, HttpHead]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages([FromQuery] MessagesResourceParameters parameters)
        {
            var result = await messagesService.MessagesRepository.GetMessagesAsync(parameters);

            return result == null ? NotFound() : (ActionResult)Ok(mapper.Map<IEnumerable<Message>>(result));
        }

        [HttpGet("{messageId:int}", Name = "GetMessageById")]
        public async Task<ActionResult<Message>> GetMessage(int messageId)
        {
            var result = await messagesService.MessagesRepository.GetByIdAsync(messageId);

            return result == null ? NotFound() : (ActionResult)Ok(result);
        }

        [HttpGet("{messageId:guid}", Name = "GetMessageByGuid")]
        public async Task<ActionResult<Message>> GetMessage(Guid messageId)
        {
            var result = await messagesService.MessagesRepository.GetByGuidAsync(messageId);

            return result == null ? NotFound() : (ActionResult)Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Message>> CreateMessage(MessageForCreation message)
        {
            var entity = mapper.Map<RepositoryMessage>(message);
            await messagesService.MessagesRepository.AddAsync(entity);
            await messagesService.MessagesRepository.SaveChangesAsync();

            var result = mapper.Map<Message>(entity);
            return CreatedAtRoute("GetMessageById", new { messageId = result.Id }, result);
        }

        [HttpOptions]
        public IActionResult GetMessagesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
