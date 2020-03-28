using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using ECommerce.Services;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using MessageType = ECommerce.Models.Helpers.OptionEnums.MessageType;

namespace ECommerce.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

		//private readonly IEmailSender _emailSender;
		private readonly ISmsSender _smsSender;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
			ISmsSender smsSender, RoleManager<ApplicationRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			//_emailSender = emailSender;
			_smsSender = smsSender;
			_roleManager = roleManager;
		}

		// GET: /Account/Register
		[HttpGet]
		[AllowAnonymous]
		[AutoValidateAntiforgeryToken]
		public IActionResult Register()
		{
			return View();
		}

		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel register)
		{
			if (ModelState.IsValid)
			{
				//if (!register.IsTermsAccepted)
				//{
				//	ModelState.AddModelError(String.Empty, "پذیرفتن حریم خصوصی و شرایط قوانین الزامی است.");

				//	return View(register);
				//}

				var user = new ApplicationUser
				{
					UserName = register.Mobile,
					Email = register.Mobile + Helper.EmailAddress,
					PhoneNumber = register.Mobile,
					PhoneNumberConfirmed = true,
					RegistrationDateAndTime = DateTime.UtcNow
				};

				var result = await _userManager.CreateAsync(user, register.Password);

				if (result.Succeeded)
				{
					var link = await _userManager.GenerateEmailConfirmationTokenAsync(user);

					await _smsSender.SendSmsAsync(register.Mobile, SmsTypes.Register,
						Helper.GenerateShortenCode(register.Mobile, link).ToString());

					return RedirectToAction(nameof(ConfirmMobile), new { userId = register.Mobile });
				}

				if (result.Errors.ToList().Count > 0 && result.Errors.ToList()[0].Code == "DuplicateUserName")
				{
					user = await _userManager.FindByNameAsync(register.Mobile);

					if (!await _userManager.IsEmailConfirmedAsync(user))
					{
						var link = await _userManager.GenerateEmailConfirmationTokenAsync(user);

						await _smsSender.SendSmsAsync(register.Mobile, SmsTypes.Register,
							Helper.GenerateShortenCode(register.Mobile, link).ToString());

						return RedirectToAction(nameof(ConfirmMobile), new { userId = register.Mobile });
					}

					TempData["StatusMessage"] = "شما قبلاً ثبت نام کرده اید، لطفاً وارد شوید.";
					return RedirectToAction(nameof(Login));
				}

				addErrors(result);
			}

			// If we got this far, something failed, redisplay form
			return View(register);
		}

		// GET: /Account/Login
		[HttpGet]
		[AllowAnonymous]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Login(string statusMessage = "")
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			if (TempData["StatusMessage"] != null)
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
			}

			return View();
		}

		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel login)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(login.Mobile);

				if (user?.IsBlocked == true)
				{
					return View("AccessDenied");
				}

				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var result = await _signInManager.PasswordSignInAsync(login.Mobile, login.Password, false, true);
				if (result.Succeeded)
				{
					var userRole = await _userManager.GetRolesAsync(user);

					if (userRole.Contains("Admin"))
					{
						return RedirectToAction("Index", "User", new { Area = "Admin" });
					}

					return RedirectToAction("Index", "Home");
				}

				if (result.IsNotAllowed)
				{
					user = await _userManager.FindByNameAsync(login.Mobile);

					if (!await _userManager.IsEmailConfirmedAsync(user))
					{
						var link = await _userManager.GenerateEmailConfirmationTokenAsync(user);

						await _smsSender.SendSmsAsync(login.Mobile, SmsTypes.Register,
							Helper.GenerateShortenCode(login.Mobile, link).ToString());

						return RedirectToAction(nameof(ConfirmMobile), new { userId = login.Mobile });
					}
				}

				if (result.IsLockedOut)
				{
					return View("Lockout");
				}

				ModelState.AddModelError(String.Empty, "شماره موبایل یا رمز عبور نادرست است.");
				return View(login);
			}

			// If we got this far, something failed, redisplay form
			ModelState.AddModelError(String.Empty, "خطا در ورود کاربر.");
			return View(login);
		}

		[HttpGet]
		[AllowAnonymous]
		[AutoValidateAntiforgeryToken]
		public IActionResult Lockout()
		{
			return View();
		}

		// POST: /Account/Logout
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		// GET: /Account/ConfirmMobile
		[HttpGet]
		[AllowAnonymous]
		[AutoValidateAntiforgeryToken]
		public IActionResult ConfirmMobile(string userId, bool isResetPassword = false)
		{
			if (String.IsNullOrWhiteSpace(userId))
			{
				return View("Error");
			}

			return View(new ConfirmationCodeViewModel
			{ Mobile = userId, IsConfirmed = false, IsResetPassword = isResetPassword });
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ConfirmMobile(ConfirmationCodeViewModel code)
		{
			if (ModelState.IsValid)
			{
				var link = Helper.GetLink(code.Mobile, code.ShortenCode);

				var user = await _userManager.FindByNameAsync(code.Mobile);

				if (user != null)
				{
					if (code.IsResetPassword)
					{
						return RedirectToAction(nameof(ResetPassword), new { code = link, userId = code.Mobile });
					}

					var result = await _userManager.ConfirmEmailAsync(user, link);
					if (result.Succeeded)
					{
						code.IsConfirmed = true;
					}
					else
					{
						ModelState.AddModelError(String.Empty, "کد وارد شده نادرست است");
					}
				}
				else
				{
					ModelState.AddModelError(String.Empty, "خطا در یافتن کاربر");
				}
			}
			else
			{
				ModelState.AddModelError(String.Empty, "خطا در درخواست");
			}

			return View(code);
		}

		// GET: /Account/ForgotPassword
		[HttpGet]
		[AllowAnonymous]
		[AutoValidateAntiforgeryToken]
		public IActionResult ForgotPassword()
		{
			return View(new ForgotPasswordViewModel { IsResetPassword = true });
		}

		// POST: /Account/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(forgotPassword.Mobile);
				if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
				{
					// Don't reveal that the user does not exist or is not confirmed
					return RedirectToAction(nameof(ConfirmMobile), new { userId = forgotPassword.Mobile, isResetPassword = forgotPassword.IsResetPassword });
				}

				var link = await _userManager.GeneratePasswordResetTokenAsync(user);

				await _smsSender.SendSmsAsync(forgotPassword.Mobile, SmsTypes.RecoverPassword,
					Helper.GenerateShortenCode(forgotPassword.Mobile, link).ToString());

				return RedirectToAction(nameof(ConfirmMobile),
					new { userId = forgotPassword.Mobile, isResetPassword = forgotPassword.IsResetPassword });
			}

			// If we got this far, something failed, redisplay form
			return View(forgotPassword);
		}

		// GET: /Account/ResetPassword
		[HttpGet]
		[AllowAnonymous]
		[AutoValidateAntiforgeryToken]
		public IActionResult ResetPassword(string code = "", string userId = "")
		{
			return String.IsNullOrWhiteSpace(code) || String.IsNullOrWhiteSpace(userId)
				? View("AccessDenied")
				: View(new ResetPasswordViewModel { Code = code, Mobile = userId });
		}

		// POST: /Account/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
		{
			if (!ModelState.IsValid)
			{
				return View(resetPassword);
			}

			var user = await _userManager.FindByNameAsync(resetPassword.Mobile);
			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
			}

			var result = await _userManager.ResetPasswordAsync(user, resetPassword.Code, resetPassword.Password);
			if (result.Succeeded)
			{
				return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
			}

			addErrors(result);
			return View();
		}

		//
		// GET: /Account/ResetPasswordConfirmation
		[HttpGet]
		[AllowAnonymous]
		[AutoValidateAntiforgeryToken]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		// GET: /Account/AccessDenied
		[HttpGet]
		[AutoValidateAntiforgeryToken]
		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[AllowAnonymous]
		[AcceptVerbs("Get", "Post")]
		public IActionResult VerifyMobile(string mobile)
		{
			if (!Helper.IsMobileNumberValid(mobile))
			{
				return Json($"شماره موبایل معتبر نیست");
			}

			return Json(true);
		}

		#region Helpers

		private void addErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(String.Empty, error.Description);
			}
		}

		#endregion Helpers
	}
}