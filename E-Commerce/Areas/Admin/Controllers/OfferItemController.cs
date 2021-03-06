﻿using ECommerce.Data;
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
		public async Task<IActionResult> AddEdit(string id)
		{
			ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");
			ViewBag.Offers = new SelectList(await _context.Offers.ToListAsync(), "Id", "Title");

			var offerItem = await _context.OfferItems.FirstOrDefaultAsync(c => c.Id == id);
			if (offerItem != null)
			{
				return PartialView("AddEdit", offerItem);
			}

			return PartialView("AddEdit", new OfferItem());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEdit(string id, OfferItem model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					await _context.OfferItems.AddAsync(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.OfferItems.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");
			ViewBag.Offers = new SelectList(await _context.Offers.ToListAsync(), "Id", "Title");

			return PartialView("AddEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var OfferItem = await _context.OfferItems.Include(x => x.Product).Include(x => x.Offer).FirstOrDefaultAsync(c => c.Id == id);
			if (OfferItem == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("Delete", $"{OfferItem.Offer?.Title} {OfferItem.Product?.Name}");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, string redirectUrl)
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