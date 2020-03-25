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
	public class UserGroupController : Controller
	{
		private readonly ApplicationDbContext _context;

		public UserGroupController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.UserGroups.ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> AddEditUserGroup(string id)
		{
			await using (_context)
			{
				var userGroup = await _context.UserGroups.FirstOrDefaultAsync(c => c.Id == id);
				if (userGroup != null)
				{
					return PartialView("AddEditUserGroup", userGroup);
				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditUserGroup(string id, UserGroup model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					await using (_context)
					{
						_context.UserGroups.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
				else
				{
					await using (_context)
					{
						_context.UserGroups.Update(model);
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

				return PartialView("AddEditUserGroup", model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> DeleteUserGroup(string id)
		{
			var userGroup = new UserGroup();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					userGroup = await _context.UserGroups.FirstOrDefaultAsync(c => c.Id.ToString() == id);
					if (userGroup == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteUserGroup", userGroup.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteUserGroup(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				await using (_context)
				{
					var model = await _context.UserGroups.FirstOrDefaultAsync(c => c.Id.ToString() == id);
					_context.UserGroups.Remove(model);
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