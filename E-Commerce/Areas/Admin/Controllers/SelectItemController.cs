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
	public class SelectItemController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SelectItemController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.SelectItems.ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEditSelectItem(string id)
		{
			ViewBag.SelectGroups = new SelectList(await _context.SelectGroups.ToListAsync(), "Id", "Name");

			var selectItem = await _context.SelectItems.FirstOrDefaultAsync(c => c.Id == id);
			if (selectItem != null)
			{
				return PartialView("AddEditSelectItem", selectItem);
			}

			return PartialView("AddEditSelectItem", new SelectItem());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditSelectItem(string id, SelectItem model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.SelectItems.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.SelectItems.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.SelectGroups = new SelectList(await _context.SelectGroups.ToListAsync(), "Id", "Name");

			return PartialView("AddEditSelectItem", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> DeleteSelectItem(string id)
		{
			var selectItem = await _context.SelectItems.FirstOrDefaultAsync(c => c.Id == id);
			if (selectItem == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeleteSelectItem", selectItem.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteSelectItem(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var selectItem = await _context.SelectItems.SingleOrDefaultAsync(a => a.Id == id);

				_context.SelectItems.Remove(selectItem);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}