using System;
using System.Net;
using System.Net.WebSockets; 
using System.Threading.Tasks;
using ChatAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Constollers
{
    public class WebSocketController :ControllerBase
    {
        private WebsocketService _websocketService;
        
        int id = new Random().Next(100);
        public WebSocketController(WebsocketService websocketService){
            Console.WriteLine("WebSocketController ======= "+id);
            _websocketService = websocketService;
        }
        [HttpGet("/ws")]
        public async Task Get([FromQuery(Name = "topics")] string topics)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await
                                  HttpContext.WebSockets.AcceptWebSocketAsync();
                var handler = new ChatAPI.Controllers.WebSocketHandler(webSocket, id);
                handler.SetTopic(topics.Split(","));
                _websocketService.AddConnection(handler);
                await handler.Echo();
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        
    }
}