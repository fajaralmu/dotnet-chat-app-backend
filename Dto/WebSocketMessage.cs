namespace ChatAPI.Dto
{
    public class WebSocketMessage<T>
    {
        public T data {get; set;}
        public string topic {get; set;}
    }
}