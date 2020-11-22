using Net5Template.Application.Services.Logs.Queries;
using Net5Template.Core.Bus;
using Net5Template.Core.Bus.RabbitMQ;
using Net5Template.Core.MessageContracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion(ApiVersions.V1)]
    [Route(ApiRouteTemplate.ROUTE_ENTITY)]
    [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
    [ApiExplorerSettings(GroupName = "Logs")]
    [ApiController]
    public class LogsController : Net5TemplateControllerBase
    {
        private readonly IMessagePublish _messagePublish;

        public LogsController(ILogger<LogsController> logger, ICommandBus command, IQueryBus query, IMessagePublish messagePublish)
            : base(logger, command, query)
        {
            _messagePublish = messagePublish;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GetLogsDTO>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentSubscriptor()
        {
            _logger.LogWarning("Test log");
            var logs = await _queryBus.Send(new GetLogsQuery());
            return Ok(logs);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GetLogsDTO>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostMessage([Required] string recipeName)
        {
            _logger.LogWarning("Test log");
            await _messagePublish.Publish(new RecipeWasChangedV1() { RecipeName = recipeName });
            return Ok();
        }

    }
}
