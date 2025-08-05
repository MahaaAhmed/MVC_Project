using Demo.DAL.Data.Repositries.Interfacies;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Repositries.Classes
{
    // primary constructor  .Net 8 c#12
    public class DepartmentRepository(AppDbContext dbContext) : GenericRepository<Department>(dbContext), IDepartmentRepository
    {

      
    }
}
