﻿using ECommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class StatusController : Controller
	{
		private readonly ApplicationDbContext _context;

		public StatusController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Statuses.ToListAsync());
		}

		//[HttpGet]
		//[AutoValidateAntiforgeryToken]
		//public async Task<IActionResult> AddEdit(string id)
		//{
		//	var status = await _context.Statuses.FirstOrDefaultAsync(c => c.Id == id);
		//	if (status != null)
		//	{
		//		return PartialView("AddEdit", status);
		//	}

		//	return PartialView("AddEdit", new Status());
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> AddEdit(string id, Status model, string redirectUrl)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		if (String.IsNullOrWhiteSpace(id))
		//		{
		//			_context.Statuses.Add(model);
		//			await _context.SaveChangesAsync();

		//			TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

		//			return PartialView("_SuccessfulResponse", redirectUrl);
		//		}

		//		_context.Statuses.Update(model);
		//		await _context.SaveChangesAsync();

		//		TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

		//		return PartialView("_SuccessfulResponse", redirectUrl);
		//	}

		//	return PartialView("AddEdit", model);
		//}

		//[HttpGet]
		//[AutoValidateAntiforgeryToken]
		//public async Task<IActionResult> Delete(string id)
		//{
		//	var status = await _context.Statuses.FirstOrDefaultAsync(c => c.Id == id);
		//	if (status == null)
		//	{
		//		return RedirectToAction("Index");
		//	}

		//	return PartialView("Delete", status.Title);
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Delete(string id, string redirectUrl)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var model = await _context.Statuses.FirstOrDefaultAsync(c => c.Id == id);

		//		_context.Statuses.Remove(model);
		//		_context.SaveChanges();

		//		TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
		//		return PartialView("_SuccessfulResponse", redirectUrl);
		//	}

		//	TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

		//	return RedirectToAction("Index");
		//}
	}
}