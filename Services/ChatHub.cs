using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Services
{
    public class ChatHub : Hub
    {
        private readonly WebsocketService websocketService;
        public ChatHub(WebsocketService websocketService)
        {
            this.websocketService = websocketService;
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Context.ConnectionId: "+Context.ConnectionId);
            Console.WriteLine("Name: "+Context.User.Identity.Name);
           
            // Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, object message)
        {
            Console.WriteLine("CHAT HUB SendMessage: "+user+" message: "+ message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine("SignalR on disconnected: "+Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }



        
    }
}