using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class DepartmentViewModel
    {

        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = " Name Length must be between 5 and 100")]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        // public int EmployeeId { get; set; }
        public virtual ICollection<Employee> employees { get; set; } = new HashSet<Employee>();
    }
}
