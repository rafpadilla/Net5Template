//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Net5Template.WebAPI.Hubs
//{
//    /*
//   SERVIDOR
//   se puede llamar desde un controlador
//       public IHubContext<ChatHub, IChatClient> _strongChatHubContext { get; }

//   public ChatController(IHubContext<ChatHub, IChatClient> chatHubContext)
//   {
//       _strongChatHubContext = chatHubContext;
//   }

//   public async Task SendMessage(string message)
//   {
//       await _strongChatHubContext.Clients.All.ReceiveMessage(message);
//   }


//   CLIENTE
//   mejor así:
//       connection.invoke("GetTotalLength", { param1: "value1", param2: "value2" });
//   */
//    //public interface IClientHubNet5Template
//    //{
//    //    Task IncomingTextMessage(string message);
//    //}
//}
