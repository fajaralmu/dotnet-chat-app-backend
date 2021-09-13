using System;
using System.Linq;
using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Context
{
    public class BaseContext<T> : DbContext where T : BaseModel
    {
        public BaseContext(DbContextOptions options) : base(options)
        {

        }
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