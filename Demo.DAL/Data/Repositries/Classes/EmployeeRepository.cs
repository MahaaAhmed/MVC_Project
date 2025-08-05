using Demo.DAL.Data.Repositries.Interfacies;
using Demo.DAL.Models;
using Demo.DAL.Models.EmployeeModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Repositries.Classes
{
    public class EmployeeRepository(AppDbContext dbContext) : GenericRepository<Employee>(dbContext), IEmployeeRepository
    {
        public IQueryable<Employee> GetEmployeeByName(string name)
        {
            return dbContext.Employees.Where(E => E.Name.ToLower().Contains(name));
        }
    }
}
