using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ChatAPI.Dto;
using ChatAPI.Helper;
using ChatAPI.Services;

namespace ChatAPI.Controllers
{
    public class WebSocketHandler
    {
        private readonly WebSocket _webSocket;
        private string[] topics = { };
        public readonly int ID;
        private readonly WebsocketService _websocketManager;
        static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

        public WebSocketHandler(WebSocket webSocket, int id, WebsocketService manager)
        {

            ID = id;
            _webSocket = webSocket;
        }

        public void SetTopic(string[] topics) => this.topics = topics;

        public async Task Echo()
        {
            var bufferInitial = new byte[1024 * 4];
            WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(bufferInitial), CancellationToken.None);
            PrintReceivedMessage(bufferInitial);

            while (!result.CloseStatus.HasValue)
            {
                var buffer = new byte[1024 * 4];
                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                PrintReceivedMessage(buffer);
            }
            await _webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private void PrintReceivedMessage(byte[] buffer)
        {
            buffer = StringUtils.TrimEnd(buffer);
            string messageAsString = StringUtils.BufferToString(buffer); 
            Console.WriteLine($">>>> RECEIVED ws message from {ID}: \n"+messageAsString+"\n");
        }

        internal bool HasTopic(string topic)
        {
            foreach (var t in topics)
                if (t == topic) return true;
            return false;
        }


        public void SendMessage(string topic, object message)
        {
            Console.WriteLine(ID + " >> Send to topic: " + topic);
            ArraySegment<byte> arraySegment = GenerateMessagePayload(topic, message);
            
            _webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private ArraySegment<byte> GenerateMessagePayload(string topic, object message)
        {
            WebSocketMessage<object> payload = new WebSocketMessage<object>();
            payload.topic = topic;
            payload.data = message is string ? message.ToString() : (message);
            
            string jsonString = JsonSerializer.Serialize(payload, _jsonSerializerOptions);
            var bytes = Encoding.Default.GetBytes(jsonString);
            return new ArraySegment<byte>(bytes);
        }
    }
}
