using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Models
{
    [Table("users")]
    [Index(nameof(Name), IsUnique = true)]
    public class User : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string role { get; set; } = "user";

    }
}