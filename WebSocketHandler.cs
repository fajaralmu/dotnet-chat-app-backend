using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatAPI.Dto;
using Newtonsoft.Json;

namespace ChatAPI.Controllers
{
    public class WebSocketHandler
    {
        private readonly WebSocket webSocket;
        private string[] topics = {};
        public readonly int ID;

        
        public WebSocketHandler(WebSocket webSocket, int ID){
            this.webSocket = webSocket; 
            this.ID = ID;
        }
    
        public void SetTopic(string[] topics)
        {
            this.topics = topics;
        }
        

        public async Task Echo()
        { 
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), 
                    CancellationToken.None);
                 var str = System.Text.Encoding.Default.GetString(buffer);
                
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
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
            Console.WriteLine(ID+" >> Send to topic: "+topic);
            WebSocketMessage<object> payload = new WebSocketMessage<object>();
            payload.topic = topic;
            payload.data = message is string ? message.ToString() :(message);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var bytes = Encoding.Default.GetBytes(JsonConvert.SerializeObject(payload));
            var arraySegment = new ArraySegment<byte>(bytes);
            webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}