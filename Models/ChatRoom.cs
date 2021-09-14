using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Models
{
    [Table("chat_room")]
    [Index(nameof(Code), IsUnique = true)]
    public class ChatRoom:BaseModel
    { 
         [Column("code"), Required]
         public string Code {get; set;}
         [Column("description"), Required]
         public string Description;

        [Column("user_id"), Required, ForeignKey("User")]
        public int UserID {get;set;}
         public User User{get;set;}
    }
}