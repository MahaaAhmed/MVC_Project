using Demo.DAL.Models;
using Demo.PL.Utilities;
using Demo.PL.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
    {
        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if(ModelState.IsValid) // server side validation
            {
                var user = new ApplicationUser()
                {
                    UserName = viewModel.Email.Split("@")[0], // mahaahmed@gmail.com  =>mahaahmed
                    Email = viewModel.Email,
                    IsAgree = viewModel.IsAgree,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,

                };
                var result = userManager.CreateAsync(user ,viewModel.Password).Result;
                if (result.Succeeded)
                    return RedirectToAction("Login");
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(viewModel);

                }
            }
            return View(viewModel);
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login()

        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if(!ModelState.IsValid) return View(viewModel);
            var user = userManager.FindByEmailAsync(viewModel.Email).Result;
            if(user is not null)
            {
                bool flag = userManager.CheckPasswordAsync(user,viewModel.Password).Result;
                if(flag)
                {
                    var Result = signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false).Result;
                    if (Result.IsNotAllowed)
                        ModelState.AddModelError(string.Empty, "Your Account Is Not Allowed");
                    if (Result.IsLockedOut)
                        ModelState.AddModelError(string.Empty, "Your Account is Locked out");
                    if(Result.Succeeded)
                        return RedirectToAction(nameof(HomeController.Index) , "Home");


                }
                

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
            return View(viewModel);

        }
        #endregion

        #region SignOut
        public async Task<IActionResult> SignOut()
        {
           await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SendResetPasswordLink(ForgetPasswordViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var user = userManager.FindByEmailAsync(viewModel.Email).Result;
                if(user is not null)
                {
                    var Token = userManager.GeneratePasswordResetTokenAsync(user).Result;
                    // baseURL/Account/ResetPassword/routemaha@gmail.com/token 
                    var ResetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = viewModel.Email, Token }, Request.Scheme);
                    // create Email 

                    var email = new Email()
                    {
                        To = viewModel.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordUrl //TODO 
                    };
                    // send Email 
                    EmailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");


                }
               

            }
            ModelState.AddModelError(string.Empty, "Invalid Operation");
            return View(nameof(ForgetPassword), viewModel);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        [HttpGet]
        //https://localhost:7021/Account/ResetPassword?email=mahaahmed5553@gmail.com&Token=CfDJ8AHEuXNP2a1MjbREzLq5pcR8yBPxLkKjnqJFFJxfpomIomrd6FnSHUoHtwxyALt%2FrU%2BN5SpaCBgECRdNmWVZeTDADMxWrKm3iZ%2BRxHJjyl8UGOPFcCAonAP9UujYZ2qSJTNfca9jlYe4XCcZo8Cp%2F5swDMzw67146run4Br0J7ElPsrbxKSAeKvrOxH497t1KboXkSY18ZgVkIxBwz9l%2Fflnu5qGWKAcw8nZwzHLBPa1
        public IActionResult ResetPassword(string email, string Token) 
        {

            TempData["email"] = email;
            TempData["Token"]= Token;
            return View();
         }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            string email = TempData["email"] as string ?? string.Empty;
            string Token = TempData["Token"] as string ?? string.Empty;

            var user = userManager.FindByEmailAsync(email).Result;
            if(user != null)
            {
                var Result = userManager.ResetPasswordAsync(user, Token, viewModel.Password).Result;
                if (Result.Succeeded)
                {

                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var item in Result.Errors)
                        ModelState.AddModelError(string.Empty, item.Description);
                }


            }
            return View(nameof(ResetPassword), viewModel);
        }

        #endregion
    }
}
