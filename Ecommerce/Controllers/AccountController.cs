﻿using Ecommerce.Models;
using Ecommerce.Models.Helpers;
using Ecommerce.Models.Helpers.OptionEnums;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MessageType = Ecommerce.Models.Helpers.OptionEnums.MessageType;

namespace Ecommerce.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;

		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			//ViewData["ReturnUrl"] = returnurl;

			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					var user = await _userManager.FindByNameAsync(model.UserName);
					//string userRole = _userManager.GetRolesAsync(user).Result.Single();

					//وقتی نام کاربری و رمز عبور صحیح باشد
					if (await _userManager.IsInRoleAsync(user, "Admin"))
					{
						return RedirectToAction("Index", $"Admin/User");
					}

					return RedirectToAction("Home", $"User/HomePage");
				}

				//وقتی نام کاربری یا رمز عبور صحیح نباشد
				TempData["Notif"] = Notification.ShowNotif(MessageType.BadLogin, ToastType.red);
				ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است");
				return View(model);
			}
			//وقتی نام کاربری یا رمز عبور خالی باشد
			return View(model);
		}

		public async Task<ActionResult> Logout()
		{
			if (Request.Cookies["S#$51%^Lm*A!2@m"] != null)
			{
				Response.Cookies.Delete("S#$51%^Lm*A!2@m");
			}
			//HttpContext.Session.Clear();
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}