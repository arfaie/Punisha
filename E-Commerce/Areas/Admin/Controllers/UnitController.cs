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
	public class UnitController : Controller
	{
		private readonly ApplicationDbContext _context;

		public UnitController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Units.ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEdit(string id)
		{
			var unit = await _context.Units.FirstOrDefaultAsync(c => c.Id == id);
			if (unit != null)
			{
				return PartialView("AddEdit", unit);
			}

			return PartialView("AddEdit", new Unit());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEdit(string id, Unit model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					await _context.Units.AddAsync(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.Units.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			return PartialView("AddEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var unit = await _context.Units.FirstOrDefaultAsync(c => c.Id == id);
			if (unit == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("Delete", unit.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.Units.FirstOrDefaultAsync(c => c.Id == id);

				_context.Units.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}