using Demo.DAL.Models;
using Demo.DAL.Models.EmployeeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Repositries.Interfacies
{
    public interface IEmployeeRepository :IGenericRepository<Employee>  
    {
       IQueryable<Employee> GetEmployeeByName(string name);




    }
}
