﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> _signInManager) 
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Account account)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Index", "Instructor");
            }
            return Content("Invalid User");
        }
        [HttpGet]
        public IActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (ModelState.IsValid) 
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = register.FName;
                applicationUser.Email = register.Email;
                applicationUser.PhoneNumber = register.PhoneNumber;
                applicationUser.Address = register.Address;
                IdentityResult identityResult=await userManager.CreateAsync(applicationUser,register.Password);
                if (identityResult.Succeeded)
                {
                    await signInManager.SignInAsync(applicationUser, isPersistent: false);
                    return RedirectToAction("Index", "Student");
                }
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                
            }
            return View("Register",register);
        }

        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Register");
        }
    }
}
