using Asset.Data.Model;
using Asset.Utility;
using Asset.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Patron> _userManager;
        private readonly SignInManager<Patron> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _web;
        public AccountController(
           UserManager<Patron> userManager,
           RoleManager<IdentityRole> roleManager, IWebHostEnvironment web, SignInManager<Patron> signInManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _web = web;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOff(LoginVM model)
        {
 
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
 
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {

                    var user = await _userManager.FindByNameAsync(model.Email);
              
                    user.DateLogin = DateTime.Now;
                    await _userManager.UpdateAsync(user);

                    HttpContext.Session.SetString("ssuserName", user.FirstName);
                    //var userName = HttpContext.Session.GetString("ssuserName");

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }


        public IActionResult Login()
        {

            return View();
        }
        public async Task<IActionResult> Register()
        {
            if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Patron));
 
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM Input)
        {
            if (ModelState.IsValid)
            {
                var user = new Patron
                {
                    UserName = Input.Email,
                   Created= DateTime.Now,
                   LastName = Input.LastName,
                   FirstName=Input.FirstName,
                   Address = Input.Address,
                   Role = Input.RoleName,
                   
                };
                if (Input.ImageUrl.Length > 0 && Input.ImageUrl != null)
                {
                    var imageRoute = @"images/employees";
                    var fileName = Path.GetFileNameWithoutExtension(Input.ImageUrl.FileName);
                    var extension = Path.GetExtension(Input.ImageUrl.FileName);
                    var webroot = _web.WebRootPath;

                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                    var path = Path.Combine(webroot, imageRoute, fileName);

                    await Input.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    user.ImageUrl = "/" + imageRoute + "/" + fileName;

                }

                var result = await _userManager.CreateAsync(user, Input.Password);
  
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Input.RoleName);
                    if (!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        TempData["Admin"] = user.FirstName;
                    }
                    return RedirectToAction("Index", "Home");
                }


            }
            return View();
        }
    }
}
