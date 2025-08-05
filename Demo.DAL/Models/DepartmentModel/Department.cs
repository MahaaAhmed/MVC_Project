using Demo.DAL.Models.EmployeeModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department :BaseEntity
    {
      
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        // Navigation prop => [Many]
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>(); 

    }
}
