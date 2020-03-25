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

		public async Task<IActionResult> Index()
		{
			var model = await _context.Units.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditUnit(string id)
		{
			var unit = new Unit(); ;
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					unit = await _context.Units.FirstOrDefaultAsync(c => c.Id.ToString() == id);
					if (unit == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("AddEditUnit", unit);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditUnit(string id, Unit model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					await using (_context)
					{
						_context.Units.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
				else
				{
					await using (_context)
					{
						_context.Units.Update(model);
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

				return PartialView("AddEditUnit", model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> DeleteUnit(string id)
		{
			var unit = new Unit();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					unit = await _context.Units.FirstOrDefaultAsync(c => c.Id.ToString() == id);
					if (unit == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteUnit", unit.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteUnit(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				await using (_context)
				{
					var model = await _context.Units.FirstOrDefaultAsync(c => c.Id.ToString() == id);
					_context.Units.Remove(model);
					_context.SaveChanges();
				}

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
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