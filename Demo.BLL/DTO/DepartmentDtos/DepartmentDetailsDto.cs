using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.DTO.DepartmentDtos
{
    public class DepartmentDetailsDto
    {
        //public DepartmentDetailsDto(Department department)
        //{
        //    Id = department.Id;
        //    Name = department.Name;
        //    Description = department.Description;
        //    CreatedOn = DateOnly.FromDateTime(department.CreatedOn.Value);

        //}

        public int Id { get; set; } // Pk 
        public int CreatedBy { get; set; } // User Id
        public DateOnly CreatedOn { get; set; } // Time of create 
        public int LastModifiedBy { get; set; } // User Id
        public bool IsDeleted { get; set; } // Soft Delete
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
