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
	public class AgencyController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AgencyController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.Agencies.ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> AddEditAgency(string id)
		{
			ViewBag.Cities = new SelectList(await _context.Cities.ToListAsync());

			var agency = await _context.Agencies.FirstOrDefaultAsync(c => c.Id == id);
			if (agency != null)
			{
				return PartialView("AddEditAgency", agency);
			}

			return PartialView("AddEditAgency", new Agency());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditAgency(string id, Agency model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.Agencies.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.Agencies.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			return PartialView("AddEditAgency", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteAgency(string id)
		{
			var agency = await _context.Agencies.FirstOrDefaultAsync(c => c.Id == id);
			if (agency == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeleteAgency", agency.Title);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAgency(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var agency = await _context.Agencies.SingleOrDefaultAsync(a => a.Id == id);

				_context.Agencies.Remove(agency);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}