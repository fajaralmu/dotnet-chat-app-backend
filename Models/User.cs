using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Models
{
    [Table("users")]
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseModel
    {
        [Required, Column("name")]
        public string Name { get; set; }
        [Required, Column("email")]
        public string Email { get; set; }
        [Required, Column("password"), JsonIgnore]
        public string Password { get; set; }
        [Required, Column("role")]
        public string Role { get; set; } = "user";

        [NotMapped]
        public string Token {get;set;}

    }
}