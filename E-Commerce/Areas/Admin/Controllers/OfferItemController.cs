using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OfferItemController : Controller
	{
		private readonly ApplicationDbContext _context;

		public OfferItemController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.OfferItems.Include(x => x.Product).Include(x => x.Offer).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEditOfferItem(string id)
		{
			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.Offers = new SelectList(await _context.Offers.ToListAsync(), "Id", "Name");

			var OfferItem = await _context.OfferItems.FirstOrDefaultAsync(c => c.Id == id);
			if (OfferItem != null)
			{
				return PartialView("AddEditOfferItem", OfferItem);
			}

			return PartialView("AddEditOfferItem", new OfferItem());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditOfferItem(string id, OfferItem model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.OfferItems.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.OfferItems.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.Offers = new SelectList(await _context.Offers.ToListAsync(), "Id", "Name");

			return PartialView("AddEditOfferItem", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> DeleteOfferItem(string id)
		{
			var OfferItem = await _context.OfferItems.Include(x => x.Product).Include(x => x.Offer).FirstOrDefaultAsync(c => c.Id == id);
			if (OfferItem == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeleteOfferItem", $"{OfferItem.Offer?.Title} {OfferItem.Product?.Name}");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteOfferItem(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.OfferItems.FirstOrDefaultAsync(c => c.Id == id);

				_context.OfferItems.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}