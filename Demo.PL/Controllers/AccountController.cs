using Demo.BLL.Helper;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        //Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = registerModel.Email.Split('@')[0],
                    Email = registerModel.Email,
                };
                var result = await UserManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerModel);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(loginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(loginModel.Email);
                if (user != null)
                {
                    var password = await UserManager.CheckPasswordAsync(user, loginModel.Password);
                    if (password)
                    {
                        var result = await SignInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }

                }
            }
            return View(loginModel);
        }
        //LogOut
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> ForgetPassword(ForgetPasswordViewModel passwordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(passwordViewModel.Email);
                if (user != null)
                {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                    var PasswordResetLink = Url.Action("ResetPassword", "Account", new { Email = passwordViewModel.Email ,token= token}, Request.Scheme );
                    var Email = new Email()
                    {
                        Tittle = "Reset Password",
                        Body = PasswordResetLink,
                        ReciverEmail = passwordViewModel.Email 

                    };
                    EmailsSettings.SendEmail(Email);
                    return RedirectToAction(nameof(CompleteForgetPassword));
                }
                ModelState.AddModelError(string.Empty, "Email Isn't Found");
            }
            return View(passwordViewModel);
        }
        public IActionResult CompleteForgetPassword()
        {
            return View();
        }
        public IActionResult ResetPassword( string email , string token)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(resetPasswordViewModel.Email);
                if (user != null)
                {
                    var result = await UserManager.ResetPasswordAsync(user, resetPasswordViewModel.Token, resetPasswordViewModel.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(ResetPasswordDone));
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(resetPasswordViewModel);
                }
                ModelState.AddModelError(string.Empty, "This Email is not Exist");
            }
            return View(resetPasswordViewModel);

        }
        public IActionResult ResetPasswordDone()
        {
            return View();
        }
    }
}
