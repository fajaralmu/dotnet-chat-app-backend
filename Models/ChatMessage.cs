using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ChatAPI.Models
{
    [Table("chat_message")]
    public class ChatMessage:BaseModel
    { 
        [Column("body"), Required]
        public string Body {get;set;}
        
        [ForeignKey("FromUser"), Column("to_user_id"), Required]
        public int FromUserID {get;set;}
        [ForeignKey("ToUser"), Column("to_user_id")]
        public int ToUserID {get;set;}
        [ForeignKey("Room"), Column("chat_room_id")]
        public int RoomID {get;set;}
        public User FromUser {get; set;}
        public User ToUser {get; set;}
        
        public ChatRoom Room {get; set;}
    }
}