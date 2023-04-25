using Demo.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "Maximunm length of name 50 ")]
        [MinLength(5, ErrorMessage = "Minumim length of name 5 ")]
        public string Name { get; set; }

        [Range(22, 60, ErrorMessage = "Age Must Be Between 22 and 60")]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,10}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}$", ErrorMessage = "Adress Must be in form of '123-Street-Region-City'")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        [Range(4000, 8000, ErrorMessage = "Salary Must Be Between 4000 & 8000")]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress(ErrorMessage = "Enter Email in a correct form ")]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int DepartmentId { get; set; }
        public virtual Department department { get; set; }
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }
    }
}
