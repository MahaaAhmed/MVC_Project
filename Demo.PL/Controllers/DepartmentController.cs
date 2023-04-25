using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]

    public class DepartmentController : Controller
    {
        private readonly IMapper Mapper;

        public IUnitOfWork UnitOfWork { get; }


        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {

            var department = Mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(await UnitOfWork.DepartmentRepository.GetAll());
            return View(department);
        }
        public async Task<IActionResult> Detials(int? id, string ViewName = "Detials")
        {
            if (id == null)
                return NotFound();
            var _Department = await UnitOfWork.DepartmentRepository.Get(id);
            if (_Department == null)
                return NotFound();
            var departmentVm = Mapper.Map<Department, DepartmentViewModel>(_Department);

            return View(ViewName, departmentVm);
        }
        public async Task<IActionResult> Edit(int? Id)
        {
            return await Detials(Id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // 3shan at2kd an msh hy7sl ay edit 8er mn l form 
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentViewModel departmentVm)
        {
            if (id != departmentVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var department = Mapper.Map<DepartmentViewModel, Department>(departmentVm);

                    await UnitOfWork.DepartmentRepository.update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception)
                {

                    return BadRequest();
                }
            }
            return View(departmentVm);

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVm)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = Mapper.Map<DepartmentViewModel, Department>(departmentVm);
                await UnitOfWork.DepartmentRepository.add(department);
                TempData["Message"] = "Department has been add successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVm);
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            return await Detials(Id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? Id, DepartmentViewModel departmentVm)
        {
            if (Id != departmentVm.Id)
                return BadRequest();
            try
            {
                var department = Mapper.Map<DepartmentViewModel, Department>(departmentVm);
                await UnitOfWork.DepartmentRepository.delete(department);
                return RedirectToAction(nameof(Index));

            }
            catch (System.Exception)
            {
                return BadRequest();
            }


        }

    }
}
