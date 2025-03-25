using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        
        public AccountController(UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> _signInManager,RoleManager<IdentityRole> _roleManager) 
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Account account)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(account.Name);
                if(user != null)
                {
                    bool result = await userManager.CheckPasswordAsync(user, account.Password);
                    if (result)
                    {
                        await signInManager.SignInAsync(user, isPersistent: true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                    ModelState.AddModelError("", "Invalid Email Or Password");
            }
            return View("Login",account);
        }
        [HttpGet]
        public async Task<IActionResult> Register() 
        {
            var roles = await roleManager.Roles
                                         .Where(r => r.Name == "Admin" || r.Name=="HR")
                                         .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                                         .ToListAsync();

            var model = new RegisterViewModel { Roles = roles };
            return View(model);
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
                applicationUser.PasswordHash = register.Password;
                IdentityResult identityResult = await userManager.CreateAsync(applicationUser);
                //IdentityResult identityResult=await userManager.CreateAsync(applicationUser,register.Password);

                if (identityResult.Succeeded)
                {
                    IdentityResult ir= await userManager.AddToRoleAsync(applicationUser, register.SelectedRole);
                    if (ir.Succeeded)
                    { 
                        await signInManager.SignInAsync(applicationUser, isPersistent: false);
                        return RedirectToAction("Index", "Student");
                    }
                    foreach (var item in ir.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    register.Roles = await roleManager.Roles
                                         .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                                         .ToListAsync();
                    return View("Register", register);
                }
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                
            }
            register.Roles = await roleManager.Roles
                                         .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                                         .ToListAsync();
            return View("Register",register);
        }

        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Register");
        }
    }
}
