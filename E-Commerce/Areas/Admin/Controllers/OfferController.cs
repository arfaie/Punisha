using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

		public async Task<IActionResult> Index()
		{
			var model = await _context.Offers.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditOffer(string id)
		{
			var offer = new Offer();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					offer = await _context.Offers.Where(b => b.Id == id).SingleOrDefaultAsync();
					if (offer == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("AddEditOffer", offer);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditOffer(string id, Offer model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					await using (_context)
					{
						_context.Offers.Add(model);
						await _context.SaveChangesAsync();
					}
					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);
					return PartialView("_SuccessfulResponse", redirectUrl);
				}
				else
				{
					await using (_context)
					{
						_context.Offers.Update(model);
						await _context.SaveChangesAsync();
					}
					TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);
					return PartialView("_SuccessfulResponse", redirectUrl);
				}
			}
			else
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, type: ToastType.Yellow);
				}
				else
				{
					TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, type: ToastType.Yellow);
				}
			}

			return PartialView("AddEditOffer", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteOffer(string id)
		{
			var offer = new Offer();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					offer = await _context.Offers.Where(b => b.Id == id).SingleOrDefaultAsync();
					if (offer == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteOffer", offer.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteOffer(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				await using (_context)
				{
					var model = await _context.Offers.Where(b => b.Id == id).SingleOrDefaultAsync();
					_context.Offers.Remove(model);
					await _context.SaveChangesAsync();
				}
				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}
			else
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Red);
				return RedirectToAction("Index");
			}
		}
	}
}