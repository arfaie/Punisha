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
	public class StateController : Controller
	{
		private readonly ApplicationDbContext _context;

		public StateController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.States.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditState(string id)
		{
			await using (_context)
			{
				var state = await _context.States.FirstOrDefaultAsync(s => s.Id.ToString() == id);
				if (state != null)
				{
					return PartialView("AddEditState", state);
				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditState(string id, State model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					await using (_context)
					{
						_context.States.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
				else
				{
					await using (_context)
					{
						_context.States.Update(model);
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
					TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, type: ToastType.Yellow);
				}
				else
				{
					TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, type: ToastType.Yellow);
				}

				return PartialView("AddEditState", model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Deletestate(string id)
		{
			var state = new State();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					state = await _context.States.Where(s => s.Id == id).SingleOrDefaultAsync();
					if (state == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("Deletestate", state.Name);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Deletestate(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				await using (_context)
				{
					var model = await _context.States.Where(s => s.Id == id).SingleOrDefaultAsync();

					_context.States.Remove(model);
					await _context.SaveChangesAsync();
				}
				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}
			else
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Yellow);

				return RedirectToAction("Index");
			}
		}
	}
}