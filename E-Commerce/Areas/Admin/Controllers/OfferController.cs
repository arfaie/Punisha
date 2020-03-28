using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OfferController : Controller
	{
		private readonly ApplicationDbContext _context;

		public OfferController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Offers.ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEditOffer(string id)
		{
			var offer = await _context.Offers.SingleOrDefaultAsync(b => b.Id == id);
			if (offer != null)
			{
				return PartialView("AddEditOffer", offer);
			}

			return PartialView("AddEditOffer", new Offer());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditOffer(string id, Offer model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.Offers.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.Offers.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			return PartialView("AddEditOffer", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> DeleteOffer(string id)
		{
			var offer = await _context.Offers.SingleOrDefaultAsync(b => b.Id == id);
			if (offer == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeleteOffer", offer.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteOffer(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.Offers.SingleOrDefaultAsync(b => b.Id == id);

				_context.Offers.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Red);
			return RedirectToAction("Index");
		}
	}
}