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
	public class FieldGroupController : Controller
	{
		private readonly ApplicationDbContext _context;

		public FieldGroupController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.FieldGroups.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditFieldGroup(string id)
		{
			var fieldGroup = new FieldGroup();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					fieldGroup = await _context.FieldGroups.Where(fg => fg.Id == id).SingleOrDefaultAsync();
					if (fieldGroup == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("AddEditFieldGroup", fieldGroup);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditFieldGroup(string id, FieldGroup model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					await using (_context)
					{
						_context.FieldGroups.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
				else
				{
					await using (_context)
					{
						_context.FieldGroups.Update(model);
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

				return PartialView("AddEditFieldGroup", model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> DeleteFieldGroup(string id)
		{
			var model = new FieldGroup();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					model = await _context.FieldGroups.Where(fg => fg.Id == id).SingleOrDefaultAsync();
					if (model == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteFieldGroup", model.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteFieldGroup(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				await using (_context)
				{
					var model = await _context.FieldGroups.Where(f => f.Id == id).SingleOrDefaultAsync();
					_context.FieldGroups.Remove(model);
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