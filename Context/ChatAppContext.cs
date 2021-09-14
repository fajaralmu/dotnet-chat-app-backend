using System;
using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChatAPI.Context
{
    public class ChatAppContext: DbContext
    {
        public ChatAppContext(DbContextOptions<ChatAppContext> options) : base(options)
        {

        }

        public DbSet<User> Users {get;set;}
        public DbSet<ChatMessage> ChatMessages {get;set;}
        public DbSet<ChatRoom> ChatRooms {get;set;}
        public DbSet<ApplicationProfile> ApplicationProfiles {get;set;}
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }

}