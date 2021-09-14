using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Models
{
    [Table("application_profile")]
    [Index(nameof(Code), IsUnique = true)]
    public class ApplicationProfile:BaseModel
    { 
         [Column("code"), Required]
         public string Code {get; set;}
         [Column("name"), Required]
         public string Name {get; set;}
         [Column("description"), Required]
         public string Description;

        [NotMapped]
        public string RequestID {get;set;}


         public static ApplicationProfile Default {
             get {
                 ApplicationProfile model = new ApplicationProfile(){
                     Name = "My App",
                     Code = "my_app",
                     Description = "Default App Description"
                 };
                 return model;
             }
         }

        
    }
}