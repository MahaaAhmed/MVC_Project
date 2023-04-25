using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]

    public class EmployeeController : Controller
    {
        private readonly IMapper mapper;

        public IUnitOfWork UnitOfWork { get; }

        public EmployeeController( IUnitOfWork _unitOfWork , IMapper _mapper)
        {
            UnitOfWork = _unitOfWork;
            mapper = _mapper;
        }


        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
            var employees = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>( await UnitOfWork.EmployeeRepository.GetAll());
            return View(employees);
            }
            else
            {
                var employees = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>( await UnitOfWork.EmployeeRepository.SearchEmployee(SearchValue));
                return View(employees);
            }
        }
        public async Task<IActionResult> Detials(int? id, string ViewName = "Detials")
        {
            if (id == null)
                return NotFound();
            var _employee = await UnitOfWork.EmployeeRepository.Get(id);
            if (_employee == null)
                return NotFound();
            var employeeVM = mapper.Map<Employee, EmployeeViewModel>(_employee);

            return View(ViewName, employeeVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // server side validation
            {
               employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var employee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await UnitOfWork.EmployeeRepository.add(employee);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.departments = UnitOfWork.DepartmentRepository.GetAll();

            return View(employeeVM);
        }

        public async Task <IActionResult> Edit(int? Id)
        {
            ViewBag.departments = UnitOfWork.DepartmentRepository.GetAll();

            return await Detials(Id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // 3shan at2kd an msh hy7sl ay edit 8er mn l form 
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel employeeVM)
        {
            if (id == employeeVM.Id)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                        var employee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                        await UnitOfWork.EmployeeRepository.update(employee);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (System.Exception)
                    {

                        return BadRequest();
                    }
                }
                ViewBag.departments = UnitOfWork.DepartmentRepository.GetAll();
                return View(employeeVM);

            }
            return BadRequest();

        }
        public async Task <IActionResult> Delete(int? Id)
        {
            return await Detials(Id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete([FromRoute] int? Id, EmployeeViewModel employeeVM)
        {
            if (Id != employeeVM.Id)
                return BadRequest();
            try
            {
                var employee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                DocumentSettings.DeleteFile(employee.ImageName, "Images");
                UnitOfWork.EmployeeRepository.delete(employee);
                return RedirectToAction(nameof(Index));

            }
            catch (System.Exception)
            {
                return BadRequest();
            }


        }
    }
}
