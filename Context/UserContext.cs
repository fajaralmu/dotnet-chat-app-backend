using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Context
{
    public class UserContext : BaseContext<User>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> UserItems {set; get;}
        
    }

}