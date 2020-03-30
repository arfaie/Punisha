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
	public class ProductFieldController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ProductFieldController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.ProductFields.Include(x => x.Product).Include(x => x.Field).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEdit(string id)
		{
			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.Fields = new SelectList(await _context.Fields.ToListAsync(), "Id", "Name");

			var productField = await _context.ProductFields.FirstOrDefaultAsync(c => c.Id == id);
			if (productField != null)
			{
				return PartialView("AddEdit", productField);
			}

			return PartialView("AddEdit", new ProductField());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEdit(string id, ProductField model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.ProductFields.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.ProductFields.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.Fields = new SelectList(await _context.Fields.ToListAsync(), "Id", "Name");

			return PartialView("AddEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var productField = await _context.ProductFields.Include(x => x.Product).Include(x => x.Field).FirstOrDefaultAsync(c => c.Id == id);
			if (productField == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("Delete", $"{productField.Field?.Title} {productField.Product?.Name}");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.ProductFields.FirstOrDefaultAsync(c => c.Id == id);

				_context.ProductFields.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}