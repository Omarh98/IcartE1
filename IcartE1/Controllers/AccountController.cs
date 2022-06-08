using IcartE1.Data;
using IcartE1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult LoginAdmin()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAdmin(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        //if (!String.IsNullOrEmpty(ReturnUrl))
                        //{
                        //    return LocalRedirect(ReturnUrl);
                        //}
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("", "Email or Password may be invalid.");
                    return View(loginViewModel);
                }
                ModelState.AddModelError("", "User doesn't have permission.");
                return View(loginViewModel);

            }
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAdmin()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
