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

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Categories.ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEdit(string id)
		{
			ViewBag.CategoryGroups = new SelectList(await _context.CategoryGroups.ToListAsync(), "Id", "Title");

			var category = await _context.Categories.SingleOrDefaultAsync(b => b.Id == id);
			if (category != null)
			{
				return PartialView("AddEdit", category);
			}

			return PartialView("AddEdit", new Category());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEdit(string id, Category model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					await _context.Categories.AddAsync(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.Categories.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.CategoryGroups = new SelectList(await _context.CategoryGroups.ToListAsync(), "Id", "Title");

			return PartialView("AddEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var category = await _context.Categories.SingleOrDefaultAsync(b => b.Id == id);
			if (category == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("Delete", category.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

				_context.Categories.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}