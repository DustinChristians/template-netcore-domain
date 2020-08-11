using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Services;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
using CompanyName.ProjectName.WebApi.Models.Message;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<ReadMessage>>> GetMessages([FromQuery] MessagesResourceParameters parameters)
        {
            var messages = await messagesService.MessagesRepository.GetMessagesAsync(parameters);

            return messages == null ? NotFound() : (ActionResult)Ok(mapper.Map<IEnumerable<ReadMessage>>(messages));
        }

        [HttpGet("{messageId:int}", Name = "GetMessageById")]
        public async Task<ActionResult<ReadMessage>> GetMessage(int messageId)
        {
            var message = await messagesService.MessagesRepository.GetByIdAsync(messageId);

            return message == null ? NotFound() : (ActionResult)Ok(mapper.Map<ReadMessage>(message));
        }

        [HttpGet("{messageId:guid}", Name = "GetMessageByGuid")]
        public async Task<ActionResult<ReadMessage>> GetMessage(Guid messageId)
        {
            var message = await messagesService.MessagesRepository.GetByGuidAsync(messageId);

            return message == null ? NotFound() : (ActionResult)Ok(mapper.Map<ReadMessage>(message));
        }

        [HttpPost]
        public async Task<ActionResult<ReadMessage>> PostMessage(CreateMessage createMessage)
        {
            var message = mapper.Map<Message>(createMessage);

            await messagesService.MessagesRepository.CreateAsync(message);

            var result = mapper.Map<ReadMessage>(message);

            return CreatedAtRoute("GetMessageByGuid", new { messageId = result.Guid }, result);
        }

        [HttpPut("{messageId}")]
        public async Task<ActionResult> PutMessage(int messageId, UpdateMessage updateMessage)
        {
            var message = await messagesService.MessagesRepository.GetByIdAsync(messageId);

            if (message == null)
            {
                return NotFound();
            }

            mapper.Map(updateMessage, message);

            messagesService.MessagesRepository.UpdateAsync(message);

            await messagesService.MessagesRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{messageId}")]
        public async Task<ActionResult> PatchMessage(int messageId, JsonPatchDocument<UpdateMessage> patchDocument)
        {
            var message = await messagesService.MessagesRepository.GetByIdAsync(messageId);

            if (message == null)
            {
                return NotFound();
            }

            var messageToPatch = mapper.Map<UpdateMessage>(message);
            patchDocument.ApplyTo(messageToPatch, ModelState);

            if (!TryValidateModel(messageToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(messageToPatch, message);
            messagesService.MessagesRepository.UpdateAsync(message);
            await messagesService.MessagesRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            var message = await messagesService.MessagesRepository.GetByIdAsync(messageId);

            if (message == null)
            {
                return NotFound();
            }

            messagesService.MessagesRepository.DeleteAsync(message);
            await messagesService.MessagesRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetMessagesOptions()
        {
            Response.Headers.Add("Allow", "DELETE, GET, HEAD, OPTIONS, PATCH, POST");

            return Ok();
        }

        public override ActionResult ValidationProblem(
        [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
