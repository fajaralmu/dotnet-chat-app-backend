using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ChatAPI.Dto;
using ChatAPI.Services;

namespace ChatAPI.Controllers
{
    public class WebSocketHandler
    {
        private readonly WebSocket _webSocket;
        private string[] topics = { };
        public readonly int ID;
        private readonly WebsocketService _websocketManager;


        public WebSocketHandler(WebSocket webSocket, int id, WebsocketService manager)
        {

            ID = id;
            _webSocket = webSocket;
            _websocketManager = manager;
        }

        public void SetTopic(string[] topics)
        {
            this.topics = topics;
        }


        public async Task Echo()
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                    CancellationToken.None);
                var str = System.Text.Encoding.Default.GetString(buffer);

            }
            _websocketManager.RemoveConnection(this);
            await _webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        internal bool HasTopic(string topic)
        {
            foreach (var t in topics)
            {
                if (t == topic) return true;
            }
            return false;
        }


        public void SendMessage(string topic, object message)
        {
            Console.WriteLine(ID + " >> Send to topic: " + topic);
            WebSocketMessage<object> payload = new WebSocketMessage<object>();
            payload.topic = topic;
            payload.data = message is string ? message.ToString() : (message);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string jsonString = JsonSerializer.Serialize(payload, options);
            var bytes = Encoding.Default.GetBytes(jsonString);
            var arraySegment = new ArraySegment<byte>(bytes);
            _webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}