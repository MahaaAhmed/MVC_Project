using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]

    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }


        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                return View(UserManager.Users);
            }
            else
            {
                var user = await UserManager.FindByEmailAsync(SearchValue);
                return View(new List<ApplicationUser> { user });
            }
        }
        public async Task<IActionResult> Detials(string id, string ViewName = "Detials")
        {
            if (id == null)
                return NotFound();
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)

                return NotFound();

            return View(ViewName, user);
        }

        public async Task<IActionResult> Edit(string Id)
        {

            return await Detials(Id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // 3shan at2kd an msh hy7sl ay edit 8er mn l form 
        public async Task<IActionResult> Edit([FromRoute] string id, ApplicationUser applicationUser)
        {
            if (id == applicationUser.Id)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await UserManager.FindByIdAsync(id);
                        user.UserName = applicationUser.UserName;
                        user.NormalizedUserName = applicationUser.UserName.ToUpper();
                        user.PhoneNumber = applicationUser.PhoneNumber;
                        var result = await UserManager.UpdateAsync(user);

                        return RedirectToAction(nameof(Index));
                    }
                    catch (System.Exception)
                    {

                        return BadRequest();
                    }
                }
                return View(applicationUser);

            }
            return BadRequest();

        }


        public async Task<IActionResult> Delete(string Id)
        {
            return await Detials(Id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string Id, ApplicationUser applicationUser)
        {
            if (Id != applicationUser.Id)
                return BadRequest();
            try
            {
                var result = await UserManager.DeleteAsync(applicationUser);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (System.Exception)
            {
                throw;
            }
            return View(applicationUser);

        }
    }
}
