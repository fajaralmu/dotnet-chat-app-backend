using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using ChatAPI.Controllers;

namespace ChatAPI.Services
{
    public class WebsocketService
    {
        private List<WebSocketHandler> _socketConnections = new List<WebSocketHandler>();

        public void AddConnection(WebSocketHandler handler)
        {
            Console.WriteLine("+++++++++ Add WS connection "+handler.ID);
            _socketConnections.Add(handler);

            Console.WriteLine("Connections count: "+_socketConnections.Count);
        }
        public void RemoveConnection(WebSocketHandler handler)
        {
            Console.WriteLine("--------- Remove WS connection "+handler.ID);
            _socketConnections.Remove(handler);

            Console.WriteLine("Connections count: "+_socketConnections.Count);
        }

        public void SendToAll(string topic, object message)
        { 
            Console.WriteLine($"Send topic `{topic}`: "+message);
            foreach (WebSocketHandler handler in  _socketConnections)
            {
                if (handler.HasTopic(topic) == false) 
                    continue;
                 
                handler.SendMessage(topic, message);
                
            }
        }
    }
}