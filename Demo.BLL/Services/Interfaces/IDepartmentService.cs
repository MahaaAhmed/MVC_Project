using Demo.BLL.DTO.DepartmentDtos;

namespace Demo.BLL.Services.Interfaces
{
    public interface IDepartmentService
    {
        int AddDepartment(CreatedDepartmentDto departmentDto);
        bool DeleteDepartment(int id);
        IEnumerable<DepartmentDto> GetAllDepartments();
        DepartmentDetailsDto? GetDepaermentById(int id);
        int UpdateDepartment(UpdateDepartmentDto departmentDto);
    }
}