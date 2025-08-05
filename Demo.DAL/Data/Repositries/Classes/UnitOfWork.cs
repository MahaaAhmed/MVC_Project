using Demo.DAL.Data.Repositries.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Repositries.Classes
{
    public class UnitOfWork : IUnitOfWork 
    {
        private Lazy<IDepartmentRepository> _DepartmentRepository;
        private Lazy<IEmployeeRepository> _EmployeeRepository;
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _DepartmentRepository = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(dbContext));
            _EmployeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(dbContext));
            _dbContext = dbContext;
        }

        public IEmployeeRepository EmployeeRepository { 
            get
            {
                return _EmployeeRepository.Value;
            }
            
            
        }
        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                return _DepartmentRepository.Value;
            }
            

        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        //public void Dispose()
        //{
        //    _dbContext.Dispose();
        //}
    }
}
