using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AddressController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public AddressController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Addresses.Include(x => x.User).Include(x => x.City).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEditAddress(string id)
		{
			ViewBag.Cities = new SelectList(await _context.Cities.ToListAsync(), "Id", "Name");
			ViewBag.Users = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");

			var address = await _context.Addresses.FirstOrDefaultAsync(c => c.Id == id);
			if (address != null)
			{
				return PartialView("AddEditAddress", address);
			}

			return PartialView("AddEditAddress", new Address());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditAddress(string id, Address model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
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

			ViewBag.SelectGroups = new SelectList(await _context.SelectGroups.ToListAsync(), "Id", "Name");

			ViewBag.Cities = new SelectList(await _context.Cities.ToListAsync(), "Id", "Name");
			ViewBag.Users = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");

			return PartialView("AddEditAddress", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> DeleteAddress(string id)
		{
			var address = await _context.Addresses.FirstOrDefaultAsync(c => c.Id == id);
			if (address == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeletAddress", address.Description);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteAddress(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var address = await _context.Addresses.SingleOrDefaultAsync(a => a.Id == id);

				_context.Addresses.Remove(address);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}