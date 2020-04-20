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
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
	public class UserProfileController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			return View(await _context.Users.Include(u => u.Car).Include(u => u.UserGroup)
				.FirstOrDefaultAsync(u => u.Id == user.Id));
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> EditProfile()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			var select = await _context.Users.Include(u => u.UserGroup).Include(u => u.Car)
				.FirstOrDefaultAsync(u => u.Id == user.Id);

			ViewBag.Cars = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name");
			ViewBag.Makers = new SelectList(await _context.Makers.ToListAsync(), "Id", "Name");

			return PartialView("EditProfile", select);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditProfile(ApplicationUser model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);

				if (user == null)
				{
					TempData["Notification"] = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red);
				}

				user.FullName = model.FullName;
				user.NationalCode = model.NationalCode;
				user.Email = model.Email;
				user.CarId = model.CarId;
				user.PhoneNumber = model.PhoneNumber;

				if (user.PhoneNumber != model.PhoneNumber)
				{
					await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
					user.UserName = model.PhoneNumber;
					user.Email = model.PhoneNumber + Helper.EmailAddress;
					user.PhoneNumberConfirmed = true;
					user.EmailConfirmed = true;
				}

				await _userManager.UpdateAsync(user);

				return RedirectToAction("Index");
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> ChangePasswordProfile()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			return PartialView("ChangePasswordProfile", new AdminChangePasswordViewModel { Id = user.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePasswordProfile(AdminChangePasswordViewModel model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
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
					return PartialView("ChangePasswordProfile", model);
				}

				//await _signInManager.RefreshSignInAsync(user);
				TempData["Notification"] = Notification.ShowNotif("رمز عبور شما با موفقیت ویرایش شد.", ToastType.Green);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			return PartialView("ChangePasswordProfile", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> userAddress()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			var UserAdrresses =
				await _context.Addresses.Where(a => a.UserId == user.Id).Include(a => a.User).Include(a => a.City).ToListAsync();

			ViewBag.UserFullName = user.FullName;
			ViewBag.UserMobile = user.PhoneNumber;

			return View(UserAdrresses);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEditUserAddress(string id)
		{
			var address = await _context.Addresses.FirstOrDefaultAsync(c => c.Id == id);

			ViewBag.States = new SelectList(await _context.States.ToListAsync(), "Id", "Name");
			ViewBag.Cities = new SelectList(await _context.Cities.ToListAsync(), "Id", "Name");

			if (address != null)
			{
				return View(address);
			}

			return View(new Address());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditUserAddress(string id, Address model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
				model.UserId = user.Id;

				ViewBag.UserFullName = user.FullName;
				ViewBag.UserMobile = user.PhoneNumber;

				if (string.IsNullOrWhiteSpace(id))
				{
					_context.Addresses.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.Addresses.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			if (string.IsNullOrWhiteSpace(id))
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, ToastType.Red);
			}
			else
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, ToastType.Red);
			}

			return View(model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> DeleteUserAddress(string id)
		{
			var select = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
			if (select != null)
			{
				_context.Addresses.Remove(select);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
				return RedirectToAction("userAddress");
			}

			return RedirectToAction("userAddress");
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> userOrders()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			ViewBag.UserFullName = user.FullName;
			ViewBag.UserMobile = user.PhoneNumber;

			return View(await _context.Orders.Where(o => o.Factor.UserId == user.Id).Include(o => o.Status).Include(o => o.Factor).OrderByDescending(x => x.TransactionDate).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> orderDetaile(string id)
		{
			ViewBag.path = "/upload/normalimage/";

			ViewBag.barasi = "";
			ViewBag.amadesazi = "";
			ViewBag.post = "";
			ViewBag.moshtari = "";

			var select = await _context.Orders.Include(o => o.Status).Include(o => o.Factor).ThenInclude(o => o.Address)
				.ThenInclude(o => o.City).Include(x => x.Factor).ThenInclude(x => x.FactorItems).ThenInclude(x => x.Product)
				.FirstOrDefaultAsync(o => o.Id == id);

			switch (select.StatusId)
			{
				case "6f9c65d681937c32dafcec03":
					ViewBag.barasi = "profile-order-steps-item-active";
					break;

				case "6f9c65d681937c32dafcec04":
					ViewBag.amadesazi = "profile-order-steps-item-active";
					break;

				case "6f9c65d681937c32dafcec05":
					ViewBag.post = "profile-order-steps-item-active";
					break;

				case "6f9c65d681937c32dafcec06":
					ViewBag.moshtari = "profile-order-steps-item-active";
					break;

				default:
					ViewBag.barasi = "";
					ViewBag.amadesazi = "";
					ViewBag.post = "";
					ViewBag.moshtari = "";
					break;
			}

			var user = await _userManager.GetUserAsync(HttpContext.User);

			ViewBag.UserFullName = user.FullName;
			ViewBag.UserMobile = user.PhoneNumber;

			return View(select);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> userComments()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			ViewBag.UserFullName = user.FullName;
			ViewBag.UserMobile = user.PhoneNumber;

			return View(await _context.CommentAndStars.Include(c => c.User).Include(c => c.Product)
				.Where(c => c.UserId == user.Id).ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> userCommentEdit(string id)
		{
			var select = await _context.CommentAndStars.FirstOrDefaultAsync(c => c.Id == id);

			var user = await _userManager.GetUserAsync(HttpContext.User);

			ViewBag.UserFullName = user.FullName;
			ViewBag.UserMobile = user.PhoneNumber;

			if (select == null)
			{
				return RedirectToAction("Index");
			}
			return PartialView("userCommentEdit", select);
		}

		[HttpPost]
		public async Task<IActionResult> userCommentEdittt(string id, CommentAndStar model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!string.IsNullOrWhiteSpace(id))
				{
					_context.CommentAndStars.Update(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
					return PartialView("_SuccessfulResponse", redirectUrl);
				}
			}

			return PartialView("userCommentEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> DeleteUserComment(string id)
		{
			var select = await _context.CommentAndStars.FirstOrDefaultAsync(a => a.Id == id);

			var user = await _userManager.GetUserAsync(HttpContext.User);

			ViewBag.UserFullName = user.FullName;
			ViewBag.UserMobile = user.PhoneNumber;

			if (select != null)
			{
				_context.CommentAndStars.Remove(select);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
				return RedirectToAction("userComments");
			}

			return RedirectToAction("userComments");
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> ReturnedGoods()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			if (user == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var factors = await _context.Factors.Where(x => x.UserId == user.Id).ToListAsync();

			ViewBag.UserFullName = user.FullName;
			ViewBag.UserMobile = user.PhoneNumber;

			return View(await _context.Orders.Where(o => o.Factor.UserId == user.Id).Include(o => o.Status).Include(o => o.Factor).OrderByDescending(x => x.TransactionDate).ToListAsync());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ReturnedGoods(string id, Address model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
				model.UserId = user.Id;

				ViewBag.UserFullName = user.FullName;
				ViewBag.UserMobile = user.PhoneNumber;

				if (string.IsNullOrWhiteSpace(id))
				{
					_context.Addresses.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.Addresses.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			if (string.IsNullOrWhiteSpace(id))
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, ToastType.Red);
			}
			else
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, ToastType.Red);
			}

			return View(model);
		}
	}
}