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
        public WebSocketHandler(WebSocket webSocket, int id)
        {

            ID = id;
            _webSocket = webSocket;
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
            _webSocket.SendAsync(
                GetPayload(topic, message), 
                WebSocketMessageType.Text, 
                true, 
                CancellationToken.None);
        }

        private ArraySegment<byte> GetPayload(string topic, object message)
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
            return new ArraySegment<byte>(bytes);
        }
    }
}