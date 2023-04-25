using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int? id);
        Task<int> add(T T);
        Task<int> update(T T);
        Task<int> delete(T T);
    }
}
