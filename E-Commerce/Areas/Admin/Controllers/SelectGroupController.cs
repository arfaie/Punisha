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
	public class SelectGroupController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SelectGroupController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.SelectGroups.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditSelectGroup(string id)
		{
			var selectGroup = await _context.SelectGroups.FirstOrDefaultAsync(c => c.Id == id);
			if (selectGroup != null)
			{
				return PartialView("AddEditSelectGroup", selectGroup);
			}

			return PartialView("AddEditSelectGroup", new SelectGroup());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditSelectGroup(string id, SelectGroup model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.SelectGroups.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.SelectGroups.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			return PartialView("AddEditSelectGroup", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteSelectGroup(string id)
		{
			var selectGroup = new SelectGroup();
			if (!String.IsNullOrWhiteSpace(id))
			{
				selectGroup = await _context.SelectGroups.FirstOrDefaultAsync(c => c.Id == id);
				if (selectGroup == null)
				{
					return RedirectToAction("Index");
				}
			}

			return PartialView("DeleteSelectGroup", selectGroup.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteSelectGroup(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.SelectGroups.FirstOrDefaultAsync(c => c.Id == id);
				_context.SelectGroups.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}