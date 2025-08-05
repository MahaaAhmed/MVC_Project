using Demo.DAL.Data.Repositries.Interfacies;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Repositries.Classes
{
    public class GenericRepository<TEntity>(AppDbContext _dbContext) : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        
        public void Add(TEntity Entity) // object member method
        {

            _dbContext.Set<TEntity>().Add(Entity);  // added
           // _dbContext.Add(Entity);
        }

        public void Delete(TEntity Entity)
        {
            _dbContext.Set<TEntity>().Remove(Entity); // remove locally [deleted]
        }

        public IEnumerable<TEntity> GetAll(bool withtracking = false)
        {
            if (withtracking)
            {
                return _dbContext.Set<TEntity>().Where(E=>E.IsDeleted != true).ToList();
            }
            else
                return _dbContext.Set<TEntity>().Where(E => E.IsDeleted != true).AsNoTracking().ToList();

        }

        public TEntity GetById(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
            // find<TEntity>(id)
        }

        public void Update(TEntity Entiy)
        {
            _dbContext.Set<TEntity>().Update(Entiy); // update locally [modified]
        }
    }
}
