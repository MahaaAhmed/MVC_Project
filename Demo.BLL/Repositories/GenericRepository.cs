using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MVCAppDbContext context;
       
        public GenericRepository(MVCAppDbContext context)
        {
            this.context = context;
        }
        public async Task<int> add(T item)
        {
            context.Set<T>().Add(item);
            return await context.SaveChangesAsync();
        }
        public async Task<int> delete(T item)
        {
            context.Set<T>().Remove(item);
            return await context.SaveChangesAsync();
        }
        public async Task <T> Get(int? id)
       => await context.Set<T>().FindAsync(id);

        public async Task<int> update(T item)
        {
            context.Set<T>().Update(item);
            return await context.SaveChangesAsync();
        }

      public  async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))

                 return (IEnumerable<T>) await context.Set<Employee>().Include(E => E.department).ToListAsync();
               return await context.Set<T>().ToListAsync();
        }
    }

}
