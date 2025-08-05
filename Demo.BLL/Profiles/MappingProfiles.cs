using AutoMapper;
using Demo.BLL.DTO.EmployeeDto;
using Demo.DAL.Models.EmployeeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeDto>()
                 .ForMember(dest => dest.EmpGender, options => options.MapFrom(src => src.Gender))
                 .ForMember(dest => dest.EmpType, options => options.MapFrom(src => src.EmployeeType))
                 .ForMember(dest => dest.Department, options => options.MapFrom(src => src.Department != null ? src.Department.Name : null));



            CreateMap<Employee, EmployeeDetailsDto>()
                 .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender))
                 .ForMember(dest => dest.EmployeeType, options => options.MapFrom(src => src.EmployeeType))
                 .ForMember(dest => dest.HiringDate, Options => Options.MapFrom(src => DateOnly.FromDateTime(src.HiringDate)));

            CreateMap<CreatedEmployeeDto, Employee>()
                .ForMember(dest => dest.HiringDate, opt => opt.MapFrom(src => src.HiringDate.ToDateTime(TimeOnly.MinValue)));

            CreateMap<UpdatedEmployeeDto, Employee>()
                 .ForMember(dest => dest.HiringDate, opt => opt.MapFrom(src => src.HiringDate.ToDateTime(TimeOnly.MinValue)));

        }
    }
}
