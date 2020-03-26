using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
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
			return View(await _context.Users.ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> AddUser()
		{
			var model = new ApplicationUser();
			//model.ApplicationRoles = await _roleManager.Roles.Select(r => new SelectListItem
			//{
			//	Text = r.Name,
			//	Value = r.Id
			//}).ToListAsync();

			//model.UserGroupListItems = await _context.UserGroups.Select(us => new SelectListItem
			//{
			//	Text = us.Title,
			//	Value = us.Id.ToString()
			//}).ToListAsync();

			//model.CarListItems = await _context.Cars.Select(c => new SelectListItem
			//{
			//	Text = c.Name,
			//	Value = c.Id.ToString()
			//}).ToListAsync();

			return PartialView("AddUser", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddUser(ApplicationUser model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					FullName = model.FullName,
					PhoneNumber = model.PhoneNumber,
					UserName = model.UserName,
					Email = model.Email
				};

				var result = await _userManager.CreateAsync(user, "asd123");
				if (result.Succeeded)
				{
					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
			}

			//model.ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
			//{
			//	Text = r.Name,
			//	Value = r.Id
			//}).ToList();

			//model.UserGroupListItems = await _context.UserGroups.Select(us => new SelectListItem
			//{
			//	Text = us.Title,
			//	Value = us.Id.ToString()
			//}).ToListAsync();

			//model.CarListItems = await _context.Cars.Select(c => new SelectListItem
			//{
			//	Text = c.Name,
			//	Value = c.Id.ToString()
			//}).ToListAsync();

			return PartialView("AddUser", model);
		}

		[HttpGet]
		public async Task<IActionResult> EditUser(string id)
		{
			var model = new ApplicationUser();
			//model.ApplicationRoles = await _roleManager.Roles.Select(r => new SelectListItem
			//{
			//	Text = r.Name,
			//	Value = r.Id
			//}).ToListAsync();

			/////////////////////////////////////////////////////////
			if (!string.IsNullOrWhiteSpace(id))
			{
				var user = await _userManager.FindByIdAsync(id);
				if (user != null)
				{
					model.Id = user.Id;
					model.FullName = user.FullName;
					model.Email = user.Email;
					model.UserName = user.UserName;
					//model.ApplicationRoleId = _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single()).Id;
					model.CarId = user.CarId;
					model.UserGroupId = user.UserGroupId;
					model.PhoneNumber = user.PhoneNumber;
				}
			}

			//model.UserGroupListItems = await _context.UserGroups.Select(us => new SelectListItem
			//{
			//	Text = us.Title,
			//	Value = us.Id.ToString()
			//}).ToListAsync();

			//model.CarListItems = await _context.Cars.Select(c => new SelectListItem
			//{
			//	Text = c.Name,
			//	Value = c.Id.ToString()
			//}).ToListAsync();

			return PartialView("EditUser", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditUser(string id, ApplicationUser model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(id);
				if (user != null)
				{
					user.FullName = model.FullName;
					user.Email = model.Email;
					user.UserName = model.UserName;
					user.CarId = model.CarId;
					user.UserGroupId = model.UserGroupId;
					user.PhoneNumber = model.PhoneNumber;

					var existingRole = _userManager.GetRolesAsync(user).Result.Single();
					var exiistingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
					var result = await _userManager.UpdateAsync(user);
					if (result.Succeeded)
					{
						//if (exiistingRoleId != model.ApplicationRoleId)
						//{
						//IdentityResult roleresult = await _userManager.RemoveFromRoleAsync(user, existingRole);
						//if (roleresult.Succeeded)
						//{
						//	ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
						//	if (applicationRole != null)
						//	{
						//		IdentityResult newrole = await _userManager.AddToRoleAsync(user, applicationRole.Name);
						//		if (newrole.Succeeded)
						//		{
						TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

						return PartialView("_SuccessfulResponse", redirectUrl);
						//}
						//}
						//}
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
			}

			//model.ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
			//{
			//	Text = r.Name,
			//	Value = r.Id
			//}).ToList();

			//model.UserGroupListItems = await _context.UserGroups.Select(us => new SelectListItem
			//{
			//	Text = us.Title,
			//	Value = us.Id.ToString()
			//}).ToListAsync();

			//model.CarListItems = await _context.Cars.Select(c => new SelectListItem
			//{
			//	Text = c.Name,
			//	Value = c.Id.ToString()
			//}).ToListAsync();

			return PartialView("EditUser", model);
		}

		[HttpGet]
		public IActionResult ChangeUserPass()
		{
			return View();
		}

		//[HttpPost]
		//public async Task<IActionResult> ChangeUserPass(string id, ChangePasswordViewModel model)
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
		public async Task<IActionResult> ChangeUserPass(string id, ChangePasswordViewModel model, string redirectUrl)
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

					TempData["Notification"] = Notification.ShowNotif("خطایی رخ داد.", ToastType.Red);
					return View();
				}

				//await _signInManager.RefreshSignInAsync(user);
				TempData["Notification"] = Notification.ShowNotif("رمز عبور شما با موفقیت ویرایش شد.", ToastType.Green);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}
			return View();
		}

		public async Task<IActionResult> UserAddress(string id)
		{
			var model = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

			return View(model);
		}
	}
}