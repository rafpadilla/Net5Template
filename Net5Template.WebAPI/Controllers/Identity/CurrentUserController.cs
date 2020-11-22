using Net5Template.Application.Services.Users.Queries;
using Net5Template.Core.Bus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion(ApiVersions.V1)]
    [Route(ApiRouteTemplate.IDENTITY_ROUTE_ENTITY)]
    [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
    [ApiExplorerSettings(GroupName = "Identity")]
    [ApiController]
    public class CurrentUserController : Net5TemplateControllerBase
    {
        public CurrentUserController(ILogger<CurrentUserController> logger, ICommandBus command, IQueryBus query)
            : base(logger, command, query)
        {
        }

        /// <summary>
        /// Información del usuario actual
        /// </summary>
        /// <returns>Un objeto con toda la información del usuario</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetUserByIdDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CurrentUserInfo()
        {
            var userId = User.GetUserId();
            var user = await _queryBus.Send(new GetUserByIdQuery(userId));
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        /// <summary>
        /// Roles del usuario actual
        /// </summary>
        /// <returns>Un array con los roles del usuario</returns>
        [HttpGet]
        [Route("Roles")]
        [ProducesResponseType(typeof(IEnumerable<GetUserRolesDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CurrentUserRoles()
        {
            var userId = User.GetUserId();

            var roles = await _queryBus.Send(new GetUserRolesQuery(userId));
            return Ok(roles);
        }
    }
}
