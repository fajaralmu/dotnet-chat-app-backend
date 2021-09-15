using System;
using System.Net;
using System.Net.WebSockets; 
using System.Threading.Tasks;
using ChatAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    public class WebSocketController :ControllerBase
    {
        private WebsocketService _websocketService;
        
        readonly int _id;
        public WebSocketController(WebsocketService websocketService){
            _websocketService = websocketService;
            _id = 1000 + new Random().Next(999);
        }
        [HttpGet("/ws")]
        public async Task Get([FromQuery(Name = "topics")] string topics)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await
                                  HttpContext.WebSockets.AcceptWebSocketAsync();
                
                WebSocketHandler handler = new WebSocketHandler(webSocket, _id, _websocketService);
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