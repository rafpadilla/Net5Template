using Net5Template.Application.Services.Logs.Queries;
using Net5Template.Core.Bus;
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
using Net5Template.Application.Services.Logs.Commands;

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
        public LogsController(ILogger<LogsController> logger, ICommandBus command, IQueryBus query)
            : base(logger, command, query)
        {
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
        public async Task<IActionResult> PostMessage([Required] string message)
        {
            _logger.LogWarning("Test log");
            var id = await _commandBus.Send(new AddLogCommand(message));
            return Created(string.Empty, id);
        }

    }
}
