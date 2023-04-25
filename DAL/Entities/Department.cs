using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Department
    {

        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
       public ICollection<Employee> employees { get; set; } = new HashSet<Employee>();
    }
}
