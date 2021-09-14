using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChatAPI.Context;
using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services
{
    public abstract class BaseMasterDataService<T> where T : BaseModel
    {
        protected ChatAppContext _context;
        protected abstract DbSet<T> Items {get;}

        public BaseMasterDataService(ChatAppContext context){
            _context = context;
        }

        protected T Add(T model)
        {
            Items.Add(model);
            _context.SaveChanges();
            return model;
        }

        protected List<T> GetList(Expression<Func<T, bool>> where)
        {
            return Items.Where(where).ToList();
        }
        protected IQueryable<T> GetQuery(Expression<Func<T, bool>> where)
        {
            return Items.Where(where);
        }

        public T Delete(int id, Expression<Func<T, bool>> where = null)
        {
            IQueryable<T> queryable = Items.Where(m => m.ID == id);
            if (where != null){
                queryable.Where(where);
            }
            T model = queryable.FirstOrDefault();
            if (null == model)
                return null;
            Items.Remove(model);
            _context.SaveChanges();
            return model;
        }

        public T Read(int id)
        {
            return Items.Where(model => model.ID == id).FirstOrDefault();
        }
    }
}