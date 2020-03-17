using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {

            var query = (from u in _context.Users
                         join ug in _context.UserGroups on u.IdUserGroup equals ug.Id
                         select new UserListViewModel
                         {
                             Id = u.Id,
                             FullName = u.Firstname + " " + u.Lastname,
                             Email = u.Email,
                             UserGroupId = u.IdUserGroup,
                             UserGroupName = ug.Title
                         }).ToList();
            return View(query);

            //List<UserListViewModel> model = new List<UserListViewModel>();
            //model = await _userManager.Users.Select(u => new UserListViewModel
            //{
            //    Id = u.Id,
            //    FullName = u.Firstname + " " + u.Lastname,
            //    Email = u.Email

            //}).ToListAsync();
            //return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            UserViewModel model = new UserViewModel();
            model.ApplicationRoles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id

            }).ToListAsync();

            model.UserGroupListItems = await _context.UserGroups.Select(us => new SelectListItem
            {
                Text = us.Title,
                Value = us.Id.ToString()
            }).ToListAsync();

            model.CarListItems = await _context.Cars.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();

            return PartialView("AddUser", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserViewModel model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Username,
                    Email = model.Email,
                    IdCar = model.CarId,
                    IdUserGroup = model.UserGroupId

                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ApplicationRole approle = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                    if (approle != null)
                    {
                        IdentityResult Roleresult = await _userManager.AddToRoleAsync(user, approle.Name);
                        if (Roleresult.Succeeded)
                        {
                            TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                            return PartialView("_Succefullyresponse", redirecturl);
                        }
                    }
                }
            }

            model.ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id

            }).ToList();

            model.UserGroupListItems = await _context.UserGroups.Select(us => new SelectListItem
            {
                Text = us.Title,
                Value = us.Id.ToString()
            }).ToListAsync();

            model.CarListItems = await _context.Cars.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();

            return PartialView("AddUser", model);

        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            EditUserViewModel model = new EditUserViewModel();
            model.ApplicationRoles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToListAsync();

            /////////////////////////////////////////////////////////
            if (!string.IsNullOrEmpty(Id))
            {
                ApplicationUser user = await _userManager.FindByIdAsync(Id);
                if (user != null)
                {
                    model.Id = user.Id;
                    model.FirstName = user.Firstname;
                    model.LastName = user.Lastname;
                    model.Email = user.Email;
                    model.Username = user.UserName;
                    model.ApplicationRoleId = _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single()).Id;
                    model.CarId = user.IdCar;
                    model.UserGroupId = user.IdUserGroup;
                    model.PhoneNumber = user.PhoneNumber;
                }
            }

            model.UserGroupListItems = await _context.UserGroups.Select(us => new SelectListItem
            {
                Text = us.Title,
                Value = us.Id.ToString()
            }).ToListAsync();

            model.CarListItems = await _context.Cars.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();

            return PartialView("EditUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Firstname = model.FirstName;
                    user.Lastname = model.LastName;
                    user.Email = model.Email;
                    user.UserName = model.Username;
                    user.IdCar = model.CarId;
                    user.IdUserGroup = model.UserGroupId;
                    user.PhoneNumber = model.PhoneNumber;

                    string existingRole = _userManager.GetRolesAsync(user).Result.Single();
                    string exiistingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (exiistingRoleId != model.ApplicationRoleId)
                        {
                            IdentityResult roleresult = await _userManager.RemoveFromRoleAsync(user, existingRole);
                            if (roleresult.Succeeded)
                            {
                                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                                if (applicationRole != null)
                                {
                                    IdentityResult newrole = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                                    if (newrole.Succeeded)
                                    {
                                        TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);

                                        return PartialView("_Succefullyresponse", redirecturl);
                                    }
                                }
                            }
                        }

                        TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);

                        return PartialView("_Succefullyresponse", redirecturl);
                    }
                }
            }

            model.ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id

            }).ToList();

            model.UserGroupListItems = await _context.UserGroups.Select(us => new SelectListItem
            {
                Text = us.Title,
                Value = us.Id.ToString()
            }).ToListAsync();

            model.CarListItems = await _context.Cars.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();

            return PartialView("EditUser", model);

        }

        [HttpGet]
        public IActionResult ChangeUserPass()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> ChangeUserPass(string id, ChangePassViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //ApplicationUser user = new ApplicationUser();
        //        var user = await _userManager.FindByIdAsync(id);

        //        if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
        //        {
        //            await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        //            ViewBag.changepass = "رمز عبور شما با موفقیت تغییر کرد";
        //            return View(model);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("OldPassword", "رمز عبور فعلی صحیح نمی باشد.");
        //            return View(model);
        //        }

        //    }
        //    return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> ChangeUserPass(string id, ChangePassViewModel model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return RedirectToAction("Index");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                {

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    TempData["Notif"] = Notification.ShowNotif("خطایی رخ داد.", ToastType.red);
                    return View();
                }

                //await _signInManager.RefreshSignInAsync(user);
                TempData["Notif"] = Notification.ShowNotif("رمز عبور شما با موفقیت ویرایش شد.", ToastType.green);
                return PartialView("_Succefullyresponse", redirecturl);

            }
            return View();
        }

        public async Task<IActionResult> UserAddress(string Id)
        {
            var model = await (from addres in _context.Addresses
                join u in _context.Users on addres.IdUser equals u.Id
               
                join c in _context.Cities on addres.IdCity equals c.Id
                where u.Id == Id
                select new AddressesViewModel()
                {
                    Id = addres.Id,
                    
                    IdCity = addres.IdCity,
                    CityName = c.Name,
                    Address = addres.Address,
                    Plaque = addres.Plaque,
                    PostalCode = addres.PostalCode,
                    Mobile = addres.Mobile,
                    Tell = addres.Tell,
                    Lan = addres.Lan,
                    Lat = addres.Lat,
                    IdUser = u.Id,
                    UserFullName = u.Firstname + " " + u.Lastname
                }).SingleOrDefaultAsync();



            return View(model);
        }
    }
}