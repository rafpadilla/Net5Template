using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Hub
{
    public interface IHubNotificationService
    {
        Task NotifyUser(string userId);
    }
}
