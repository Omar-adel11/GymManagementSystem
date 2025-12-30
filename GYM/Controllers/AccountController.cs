using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.AccountsModelView;
using GYM.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    public class AccountController(IAccountService _accountService, SignInManager<ApplicationUser> _signInManager) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if(!ModelState.IsValid)
            {
                return View(login);
            }

            var user = await _accountService.ValidateUser(login);
            if(user is null)
            {
                ModelState.AddModelError("LoginInvalid","Invalid Email Or Password");
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe,false);
            if(result.IsNotAllowed)
            {
                return View(login);
                ModelState.AddModelError("LoginInvalid", "Your account is not allowed");
            }
            if(result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            return View(login); 

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
