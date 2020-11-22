using Net5Template.Application.Services.Identity.Commands;
using Net5Template.Application.Services.Users.Commands;
using Net5Template.Application.Services.Users.Queries;
using Net5Template.Core.Bus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion(ApiVersions.V1)]
    [Route(ApiRouteTemplate.ROUTE_ENTITY)]
    [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
    [ApiExplorerSettings(GroupName = "Identity")]
    [ApiController]
    public class IdentityController : Net5TemplateControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public IdentityController(ILogger<IdentityController> logger, ICommandBus command, IQueryBus query,
            IWebHostEnvironment environment)
            : base(logger, command, query)
        {
            _environment = environment;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegisterCommand model)
        {
            if (ModelState.IsValid)
            {

                var userByEmail = await _queryBus.Send(new GetUserByEmailQuery(model.Email));
                if (userByEmail != null)
                {
                    return BadRequest("Email already exists");
                }
                var userByUsername = await _queryBus.Send(new GetUserByUserNameQuery(model.Username));
                if (userByUsername != null)
                {
                    return BadRequest("Username already exists");
                }
                model.Id = Guid.NewGuid();
                var result = await _commandBus.Send(model);
                if (result.Succeeded)
                {
                    var res = await _commandBus.Send(new GenerateTokenJWTCommand(model.Id.Value, model.Fingerprint));
                    return Ok(res);
                }
                else
                {
                    return BadRequest("Username or password invalid");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPut("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLoginCommand model)
        {
            if (ModelState.IsValid)
            {
                var result = await _commandBus.Send(model);
                if (result.Succeeded)
                {
                    Guid userId = Guid.Empty;
                    if (model.UserOrEmail.Contains("@"))
                    {
                        var user = await _queryBus.Send(new GetUserByEmailQuery(model.UserOrEmail));
                        if (user != null)
                            userId = user.Id;
                    }
                    else
                    {
                        var user = await _queryBus.Send(new GetUserByUserNameQuery(model.UserOrEmail));
                        if (user != null)
                            userId = user.Id;
                    }
                    if (userId.Equals(Guid.Empty))
                        return BadRequest(ModelState);

                    var res = await _commandBus.Send(new GenerateTokenJWTCommand(userId, model.Fingerprint));
                    return Ok(res);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPut("RefreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRefreshToken(RefreshTokenCommand model)
        {
            var res = await _commandBus.Send(model);
            return Ok(res);
        }

        [HttpPut("Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout(DeleteRefreshTokenCommand deleteRefreshTokenCommand)
        {
            await _commandBus.Send(deleteRefreshTokenCommand);
            return Ok();
        }
    }
}
