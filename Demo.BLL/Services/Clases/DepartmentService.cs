using Demo.BLL.DTO.DepartmentDtos;
using Demo.BLL.Factories;
using Demo.BLL.Services.Interfaces;
using Demo.DAL.Data.Repositries.Classes;
using Demo.DAL.Data.Repositries.Interfacies;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Clases
{
    // primary constuctor
    public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository = departmentRepository;

        // Get All Departments
        public IEnumerable<DepartmentDto> GetAllDepartments()
        {
            var deprtments = _unitOfWork.DepartmentRepository.GetAll();


            // 1. Manual mapping
            //var departmentsToReturn = deprtments.Select(D => new DepartmentDto()
            //{
            //        Id = D.Id,
            //        Name = D.Name,
            //        Description = D.Description,
            //        Code = D.Code,
            //        DateOfCreation = DateOnly.FromDateTime(D.CreatedOn.Value)
            //});
            //return departmentsToReturn;

            // 2. Extension method
            return deprtments.Select(D => D.ToDepartmentDto());


        }

        // Get Department By Id
        public DepartmentDetailsDto? GetDepaermentById(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
           

            //if (department is null) return null;
            //else
            //{
            //    var departmentToReturn = new DepartmentDetailsDto()
            //    {
            //        Id = department.Id,
            //        Name = department.Name,
            //        Description = department.Description,
            //        Code = department.Code,
            //        CreatedOn = DateOnly.FromDateTime(department.CreatedOn.Value)
            //    };
            //    return departmentToReturn;
            //}

            // Manual Mapping 
            // Auto Mapper 
            // Construcor Mapping
            // Extension methods 

            // 1. manual mapping
            //return department is null ? null : new DepartmentDetailsDto(department)
            //{
            //    //Id = department.Id,
            //    //Name = department.Name,
            //    //Description = department.Description,
            //    //Code = department.Code,
            //    //CreatedOn = DateOnly.FromDateTime(department.CreatedOn.Value)
            //};

            // 2. Extension methods 
            return department is null ? null : department.ToDepartmentDetailsDto();
        }

        // Add Department 
        public int AddDepartment(CreatedDepartmentDto departmentDto)
        {
            var department = departmentDto.ToEntity();
             _unitOfWork.DepartmentRepository.Add(department);
            // 
            /// 
            // 
            return _unitOfWork.SaveChanges();
        }


        // update Department 
        public int UpdateDepartment(UpdateDepartmentDto departmentDto)
        {

             _unitOfWork.DepartmentRepository.Update(departmentDto.ToEntity());
            return _unitOfWork.SaveChanges();
        }

        // Delete Department 
        public bool DeleteDepartment(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
            if (department is null) return false;
            else
            {
                 _unitOfWork.DepartmentRepository.Delete(department);
                return _unitOfWork.SaveChanges() > 0 ? true : false;
            }
        }


    }
}
