using Demo.BLL.DTO.DepartmentDtos;
using Demo.BLL.Services.Interfaces;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController(IDepartmentService _departmentService,
        ILogger<DepartmentController> _logger, IWebHostEnvironment _environment) : Controller
    {
        //private readonly IDepartmentService _departmentService = departmentService;

        public IActionResult Index()
        {
            var departments = _departmentService.GetAllDepartments();

            return View(departments);
        }

        #region Create Department

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatedDepartmentDto departmentDto)
        {
            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    int result = _departmentService.AddDepartment(departmentDto);

                    if (result > 0)

                        return RedirectToAction(nameof(Index));
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Department can't be created !!");
                    }

                }
                catch (Exception ex)
                {
                    // log exception 
                    if (_environment.IsDevelopment())
                    {
                        // 1. Development  => Log Error in Console and return same view with error msg
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    else
                    {
                        // 2. Deployment   => Log Error  in file | Table in database And Return Error view 
                        _logger.LogError(ex.Message);

                    }



                }
            }
            return View(departmentDto);

        }
        #endregion

        #region Details of Department
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest(); // 400
            var department = _departmentService.GetDepaermentById(id.Value);
            if (department is null) return NotFound(); // 404
            return View(department);
        }
        #endregion

        #region Edit Department
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if ((!id.HasValue)) return BadRequest();
            var department = _departmentService.GetDepaermentById(id.Value);
            if (department is null) return NotFound();
            var departmentViewModel = new DepartmentEditViewModel()
            {
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                DateOfCreation = department.CreatedOn
            };
            return View(departmentViewModel);

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit([FromRoute] int? id ,DepartmentEditViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            try
            {
                var updatedDepartment = new UpdateDepartmentDto()
                {
                    Id = id.Value,
                    Code = viewModel.Code,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    DateOfCreation = viewModel.DateOfCreation
                };
                int resullt = _departmentService.UpdateDepartment(updatedDepartment);
                if (resullt > 0)
                    return RedirectToAction(nameof(Index));

                else
                {
                    ModelState.AddModelError(string.Empty, "Department can't be created !!");
                }
            }
            catch (Exception ex)
            {

                if (_environment.IsDevelopment())
                {
                    // 1. Development  => Log Error in Console and return same view with error msg
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    // 2. Deployment   => Log Error  in file | Table in database And Return Error view 
                    _logger.LogError(ex.Message);

                }
            }
            return View(viewModel);
        }
        #endregion

        #region Delete Department
        // Department/Delete
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (!id.HasValue) return BadRequest();
        //    var department = _departmentService.GetDepaermentById(id.Value);
        //    if (department == null) return NotFound();
        //    return View(department);

        //}


        // Department/Delete/10
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if(id == 0 ) return BadRequest();
            try
            {
                bool deleted = _departmentService.DeleteDepartment(id);
                if(deleted)
                    return RedirectToAction(nameof(Index));

                else
                {
                    ModelState.AddModelError(string.Empty, "Departmrnt is not deleted");
                    // data of department 
                    return RedirectToAction(nameof(Delete),new { id});
                }
            }
            catch (Exception ex)
            {

                if (_environment.IsDevelopment())
                {
                    // 1. Development  => Log Error in Console and return same view with error msg
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    // 2. Deployment   => Log Error  in file | Table in database And Return Error view 
                    _logger.LogError(ex.Message);
                    return View("Error");

                }
            }
        }
        #endregion


    }
}
