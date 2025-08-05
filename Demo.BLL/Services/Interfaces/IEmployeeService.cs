using Demo.BLL.DTO.EmployeeDto;
using Demo.DAL.Models.EmployeeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Interfaces
{
    public interface IEmployeeService
    {
        // Get All Employee
        IEnumerable<EmployeeDto> GetAllEmployees(bool withTracking = false);
        IEnumerable<EmployeeDto> SearchEmployeeByName(string name);

        EmployeeDetailsDto? GetEmployeeById(int id);
        int CreateEmployee(CreatedEmployeeDto employee);
        int UpdateEmployee(UpdatedEmployeeDto employee);
        bool DeleteEmployee(int id);
    }
}
