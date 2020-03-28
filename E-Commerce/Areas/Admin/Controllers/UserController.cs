using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
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

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Users.Include(x => x.UserGroup).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddUser()
		{
			ViewBag.ApplicationRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");

			ViewBag.UserGroups = new SelectList(await _context.UserGroups.ToListAsync(), "Id", "Title");

			ViewBag.Cars = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name");

			return PartialView("AddUser", new ApplicationUser());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddUser(ApplicationUser model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				model.RegistrationDateAndTime = DateTime.UtcNow;
				model.UserName = model.PhoneNumber;
				model.Email = model.PhoneNumber + Helper.EmailAddress;
				model.EmailConfirmed = true;
				model.PhoneNumberConfirmed = true;

				var result = await _userManager.CreateAsync(model, model.Password);
				if (result.Succeeded)
				{
					var applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
					if (applicationRole != null)
					{
						var newRole = await _userManager.AddToRoleAsync(model, applicationRole.Name);
						if (!newRole.Succeeded)
						{
							TempData["Notification"] = Notification.ShowNotif("خطا در تعیین نقش کاربر", ToastType.Red);

							return PartialView("_SuccessfulResponse", redirectUrl);
						}
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				TempData["Notification"] = Notification.ShowNotif("خطا در ثبت کاربر جدید", ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.ApplicationRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");

			ViewBag.UserGroups = new SelectList(await _context.UserGroups.ToListAsync(), "Id", "Title");

			ViewBag.Cars = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name");

			return PartialView("AddUser", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> EditUser(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				var user = await _userManager.FindByIdAsync(id);
				if (user != null)
				{
					var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

					if (!String.IsNullOrWhiteSpace(userRole))
					{
						var role = await _roleManager.FindByNameAsync(userRole);

						if (role != null)
						{
							user.ApplicationRoleId = role.Id;
						}
					}

					ViewBag.ApplicationRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", user.ApplicationRoleId);

					ViewBag.UserGroups = new SelectList(await _context.UserGroups.ToListAsync(), "Id", "Title", user.UserGroupId);

					ViewBag.Cars = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name", user.CarId);

					return PartialView("EditUser", user);
				}
			}

			return PartialView("EditUser");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditUser(ApplicationUser model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id);
				if (user == null)
				{
					TempData["Notification"] = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				user.FullName = model.FullName;
				user.NationalCode = model.NationalCode;
				user.TaxiCode = model.TaxiCode;
				user.CarId = model.CarId;
				user.UserGroupId = model.UserGroupId;

				if (user.PhoneNumber != model.PhoneNumber)
				{
					await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
					user.UserName = model.PhoneNumber;
					user.Email = model.PhoneNumber + Helper.EmailAddress;
					user.PhoneNumberConfirmed = true;
					user.EmailConfirmed = true;
				}

				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					var applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
					if (applicationRole != null)
					{
						var allRoles = await _userManager.GetRolesAsync(user);
						await _userManager.RemoveFromRolesAsync(user, allRoles);

						var newRole = await _userManager.AddToRoleAsync(user, applicationRole.Name);
						if (!newRole.Succeeded)
						{
							TempData["Notification"] = Notification.ShowNotif("خطا در تعیین نقش کاربر", ToastType.Red);

							return PartialView("_SuccessfulResponse", redirectUrl);
						}
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
			}

			var userRole = (await _userManager.GetRolesAsync(model)).FirstOrDefault();

			if (!String.IsNullOrWhiteSpace(userRole))
			{
				var role = await _roleManager.FindByNameAsync(userRole);

				if (role != null)
				{
					model.ApplicationRoleId = role.Id;
				}
			}

			ViewBag.ApplicationRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.ApplicationRoleId);

			ViewBag.UserGroups = new SelectList(await _context.UserGroups.ToListAsync(), "Id", "Title", model.UserGroupId);

			ViewBag.Cars = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name", model.CarId);

			return PartialView("EditUser", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public IActionResult ChangeUserPass(string id)
		{
			return PartialView("ChangeUserPass", new AdminChangePasswordViewModel { Id = id });
		}

		//[HttpPost] [ValidateAntiForgeryToken]
		//public async Task<IActionResult> ChangeUserPass(string id, ChangePasswordViewModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		//ApplicationUser user = new ApplicationUser();
		//		var user = await _userManager.FindByIdAsync(id);

		//		if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
		//		{
		//			await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
		//			ViewBag.changepass = "رمز عبور شما با موفقیت تغییر کرد";
		//			return PartialView("_SuccessfulResponse", redirectUrl);
		//		}
		//		else
		//		{
		//			ModelState.AddModelError("OldPassword", "رمز عبور فعلی صحیح نمی باشد.");
		//			return View(model);
		//		}
		//	}
		//	return View(model);
		//}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangeUserPass(string id, AdminChangePasswordViewModel model, string redirectUrl)
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
					return PartialView("ChangeUserPass", model);
				}

				//await _signInManager.RefreshSignInAsync(user);
				TempData["Notification"] = Notification.ShowNotif("رمز عبور شما با موفقیت ویرایش شد.", ToastType.Green);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			return PartialView("ChangeUserPass", model);
		}

		public async Task<IActionResult> UserAddress(string id)
		{
			var model = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

			return View(model);
		}
	}
}