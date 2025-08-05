using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Repositries.Interfacies
{
    public interface IGenericRepository<TEntity> where TEntity :BaseEntity
    {
        // Get All
        IEnumerable<TEntity> GetAll(bool withTracking = false);
        // Get By Id
        TEntity GetById(int id);
        // Update 
        void Update(TEntity Entiy);
        // Delete 
        void Delete(TEntity Entity);

        // Insert
        void Add(TEntity Entity);
    }
}
