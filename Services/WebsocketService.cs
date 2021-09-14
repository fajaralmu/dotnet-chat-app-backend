using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using ChatAPI.Middlewares;

namespace ChatAPI.Services
{
    public class WebsocketService
    {
        private List<WebSocketHandler> _socketConnections = new List<WebSocketHandler>();

        public void AddConnection(WebSocketHandler handler)
        {
            Console.WriteLine("+++++++= Add WS connection "+handler.ID);
            _socketConnections.Add(handler);

            Console.WriteLine("_socketConnections count: "+_socketConnections.Count);
        }
        public void RemoveConnection(WebSocketHandler handler)
        {
            Console.WriteLine("---------- Remove WS connection "+handler.ID);
            _socketConnections.Remove(handler);
        }

        public void SendToAll(string topic, object message)
        { 
            Console.WriteLine($"SendTo `{topic}`: "+message);
            foreach (WebSocketHandler handler in  _socketConnections)
            {
                if (handler.HasTopic(topic) == false) 
                    continue;
                Console.WriteLine($"SendTo {handler.ID} :"+message);
                 
                handler.SendMessage(topic, message.ToString());
                
            }
        }
    }
}