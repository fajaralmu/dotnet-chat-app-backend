namespace ChatAPI.Dto
{
    public class ChatMessageDto
    {
        public int ToUserID {get; set;}
        public int RoomID {get; set;}
        public string Body {get; set;}

    }
}