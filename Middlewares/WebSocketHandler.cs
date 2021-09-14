using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatAPI.Middlewares
{
    public class WebSocketHandler
    {
        private readonly WebSocket webSocket;
        public readonly int ID;
        public WebSocketHandler(WebSocket webSocket, int ID){
            this.webSocket = webSocket; 
            this.ID = ID;
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
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public void SendMessage(string message)
        {
            Console.WriteLine("Socket SendMessage: "+message);
            var bytes = Encoding.Default.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(bytes);
            webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}