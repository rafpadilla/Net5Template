using Net5Template.Core.Bus;
using Net5Template.Core.Hub;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Net5TemplateHub : Hub<IClientHubNet5Template>//strongtype client
    {
        private readonly ILogger<Net5TemplateHub> _logger;
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        public Net5TemplateHub(ILogger<Net5TemplateHub> logger, ICommandBus commandBus, IQueryBus queryBus)
              : base()
        {
            _logger = logger;
            _commandBus = commandBus;
            _queryBus = queryBus;
        }
        #region Override Methods
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await OnDisconnectedWebRTC();
            await base.OnDisconnectedAsync(exception);
        }
        #endregion
        #region Private Helpers
        private string HubUserName
        {
            get
            {
                return Context.User.Identity.Name;
            }
        }
        private string HubUserId
        {
            get
            {
                return Context.User.GetUserId().ToString("D");
            }
        }
        #endregion
        public Task OnDisconnectedWebRTC()
        {
            // Hang up any calls the user is in
            //await HangUp(); // Gets the user from "Context" which is available in the whole hub

            //int? targetThreadId = UserInCallHelper.GetUserHasCallerOffers(HubUserId);
            //if(targetThreadId.HasValue)
            //    UserInCallHelper.RemoveOffer

            //await _userInCallAppService.RemoveUser(Context.ConnectionId);
            return Task.CompletedTask;
        }
        public async Task Join(JoinRequest req)//string username)
        {
            //await _userOnlineAppService.AddUser(HubUserId);

            await Clients.User(HubUserId).IncomingTextMessage("net5template -> joined");
        }
    }
}
