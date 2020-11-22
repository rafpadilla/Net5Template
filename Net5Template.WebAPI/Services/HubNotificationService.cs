using Net5Template.Core.Hub;
using Net5Template.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Services
{
    public class HubNotificationService : IHubNotificationService
    {
        private readonly IHubContext<Net5TemplateHub, IClientHubNet5Template> _hubContext;

        public HubNotificationService(IHubContext<Net5TemplateHub, IClientHubNet5Template> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task NotifyUser(string userId)
        {
            await _hubContext.Clients.User(userId).IncomingTextMessage("prueba desde servidor");
        }
    }
}
