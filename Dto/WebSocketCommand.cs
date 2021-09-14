namespace ChatAPI.Dto
{
    public class WebSocketCommand
    {
        public string command {get;set;}
        public Identifier identifier {get; set;}
    }

    public class Identifier
    {
        public string[] channels {get;set;}
    }
}