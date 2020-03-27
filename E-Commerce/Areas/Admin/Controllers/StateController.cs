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
	public class StateController : Controller
	{
		private readonly ApplicationDbContext _context;

		public StateController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.States.ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> AddEditState(string id)
		{
			var state = await _context.States.FirstOrDefaultAsync(c => c.Id == id);
			if (state != null)
			{
				return PartialView("AddEditState", state);
			}

			return PartialView("AddEditState", new State());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditState(string id, State model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.States.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.States.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			return PartialView("AddEditState", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteState(string id)
		{
			var state = await _context.States.SingleOrDefaultAsync(s => s.Id == id);
			if (state == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeleteState", state.Name);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteState(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.States.SingleOrDefaultAsync(s => s.Id == id);

				_context.States.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}