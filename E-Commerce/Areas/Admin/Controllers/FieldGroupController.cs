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
	public class FieldGroupController : Controller
	{
		private readonly ApplicationDbContext _context;

		public FieldGroupController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.FieldGroups.ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> AddEditFieldGroup(string id)
		{
			var fieldGroup = await _context.FieldGroups.SingleOrDefaultAsync(fg => fg.Id == id);
			if (fieldGroup != null)
			{
				return PartialView("AddEditFieldGroup", fieldGroup);
			}

			return PartialView("AddEditFieldGroup", new FieldGroup());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditFieldGroup(string id, FieldGroup model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.FieldGroups.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.FieldGroups.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			return PartialView("AddEditFieldGroup", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteFieldGroup(string id)
		{
			var model = await _context.FieldGroups.SingleOrDefaultAsync(fg => fg.Id == id);
			if (model == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeleteFieldGroup", model.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteFieldGroup(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.FieldGroups.SingleOrDefaultAsync(f => f.Id == id);
				_context.FieldGroups.Remove(model);

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}