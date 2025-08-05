using Demo.BLL.DTO.DepartmentDtos;
using Demo.BLL.DTO.EmployeeDto;
using Demo.BLL.Services.AttachmentService;
using Demo.BLL.Services.Clases;
using Demo.BLL.Services.Interfaces;
using Demo.DAL.Models.EmployeeModel;
using Demo.PL.ViewModels;
using Demo.PL.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class EmployeeController(IEmployeeService _employeeService, IDepartmentService departmentService,
         ILogger<EmployeeController> _logger, IWebHostEnvironment _environment , IAttachmentService attachmentService) : Controller
    {

        public IActionResult Index(string? EmployeeSearchName)
        {
            //TempData.Keep();
            //Binding through view's dictionary : transfer Data From Action To View 
            // 1. ViewData
            //ViewData["Message"] = "Hello ViewData";
            //string viewDataMessage = ViewData["Message"] as string;

            //// 2. ViewBag
            //ViewBag.Message = "Hello ViewBag";
            //string viewwBagMsg = ViewBag.Message;
            dynamic Employees = null!;
            if (string.IsNullOrEmpty(EmployeeSearchName))
            {
                 Employees = _employeeService.GetAllEmployees();

            }
            else
            {
                 Employees = _employeeService.SearchEmployeeByName(EmployeeSearchName);

            }

            return View(Employees);
        }
        #region Create Employee
        [HttpGet]
        public IActionResult Create(/*[FromServices] IDepartmentService _departmentService*/)
        {
            //ViewData["Departments"] = _departmentService.GetAllDepartments();
            //ViewBag.Departments = _departmentService.GetAllDepartments(); ;
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeDto)
        {
            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var employeeCreatedDto = new CreatedEmployeeDto()
                    {
                        Name = employeeDto.Name,
                        Address = employeeDto.Address,
                        Age = employeeDto.Age,
                        IsActive = employeeDto.IsActive,
                        Email = employeeDto.Email,
                        EmployeeType = employeeDto.EmployeeType,
                        Gender = employeeDto.Gender,
                        HiringDate = employeeDto.HiringDate,
                        PhoneNumber = employeeDto.PhoneNumber,
                        Salary = employeeDto.Salary,
                        DepartmentId = employeeDto.DepartmentId,
                        Image = employeeDto.Image
                    };
                    // create  => created  , savechanges()
                    int result = _employeeService.CreateEmployee(employeeCreatedDto); // created
                                                                                          // update  => updated  , savechnges()
                                                                                          // edit departmentId  => modified  , savechanges()

                    // delete  => deleteed , savechanges()


                    // create , update , edit , delete   => savechanges() 

                    // 3. TempData 
                    if (result > 0)
                    {
                        TempData["Message"] = "Employee Created Succesfuly ";
                        return RedirectToAction(nameof(Index));

                    }



                    else
                    {
                        TempData["Message"] = "Employee Creation failed ";

                        ModelState.AddModelError(string.Empty, "Employee can't be created !!");
                        return RedirectToAction(nameof(Index));

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
            return View(employeeDto);
        }




        #endregion

        #region Details Of Employee
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest(); // 400
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound(); // 404
            return View(employee);
        }
        #endregion

        #region Edit Employee
        [HttpGet]
        public IActionResult Edit(int? id/*, [FromServices] IDepartmentService _departmentService*/)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();
            var employeeDto = new EmployeeViewModel()
            {
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                HiringDate = employee.HiringDate,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType),
                
                
            };
            //ViewData["Departments"] = _departmentService.GetAllDepartments();
            return View(employeeDto);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            try
            {
                var employeeUpdatedDto = new UpdatedEmployeeDto()
                {
                    Id = id.Value,
                    Name = viewModel.Name,
                    Address = viewModel.Address,
                    Age = viewModel.Age,
                    IsActive = viewModel.IsActive,
                    Email = viewModel.Email,
                    EmployeeType = viewModel.EmployeeType,
                    Gender = viewModel.Gender,
                    HiringDate = viewModel.HiringDate,
                    PhoneNumber = viewModel.PhoneNumber,
                    Salary = viewModel.Salary,
                };
                int resullt = _employeeService.UpdateEmployee(employeeUpdatedDto);
                if (resullt > 0)
                    return RedirectToAction(nameof(Index));

                else
                {
                    ModelState.AddModelError(string.Empty, "Employee can't be updated !!");
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

        #region Delete Employee
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                var deleted = _employeeService.DeleteEmployee(id);
                if (deleted)
                {
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee Is Not Deleted!");
                    return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));

        }
        #endregion


    }
}
