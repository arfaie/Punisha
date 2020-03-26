using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using MessageType = ECommerce.Models.Helpers.OptionEnums.MessageType;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CategoryController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.Categories.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditCategory(string id)
		{
			var category = new Category();
			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
					if (category == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("AddEditCategory", category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditCategory(string id, Category model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					{
						_context.Categories.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				{
					_context.Categories.Update(model);
					await _context.SaveChangesAsync();
				}

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			if (!String.IsNullOrWhiteSpace(id))
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, type: ToastType.Yellow);
			}
			else
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, type: ToastType.Yellow);
			}

			return PartialView("AddEditCategory", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteCategory(string id)
		{
			var category = new Category();
			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
					if (category == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteCategory", category.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCategory(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				{
					var model = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
					_context.Categories.Remove(model);
					_context.SaveChanges();
				}

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}