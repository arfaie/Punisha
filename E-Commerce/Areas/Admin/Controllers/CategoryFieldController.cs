using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoryFieldController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CategoryFieldController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.CategoryFields.Include(x => x.Category).Include(x => x.Field).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEditCategoryField(string id)
		{
			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.Fields = new SelectList(await _context.Fields.ToListAsync(), "Id", "Name");

			var categoryField = await _context.CategoryFields.FirstOrDefaultAsync(c => c.Id == id);
			if (categoryField != null)
			{
				return PartialView("AddEditCategoryField", categoryField);
			}

			return PartialView("AddEditCategoryField", new CategoryField());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditCategoryField(string id, CategoryField model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.CategoryFields.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.CategoryFields.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.Fields = new SelectList(await _context.Fields.ToListAsync(), "Id", "Name");

			return PartialView("AddEditCategoryField", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> DeleteCategoryField(string id)
		{
			var categoryField = await _context.CategoryFields.Include(x => x.Category).Include(x => x.Field).FirstOrDefaultAsync(c => c.Id == id);
			if (categoryField == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeleteCategoryField", $"{categoryField.Field?.Title} {categoryField.Category?.Title}");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCategoryField(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.CategoryFields.FirstOrDefaultAsync(c => c.Id == id);

				_context.CategoryFields.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}