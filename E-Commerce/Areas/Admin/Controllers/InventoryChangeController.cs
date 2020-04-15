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

namespace E_Commerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class InventoryChangeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public InventoryChangeController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.InventoryChanges.Include(i => i.Product).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEdit(string id)
		{
			ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");

			var select = await _context.InventoryChanges.SingleOrDefaultAsync(x => x.Id == id);
			if (select != null)
			{
				return PartialView("AddEdit", select);
			}

			return PartialView("AddEdit", new InventoryChange());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEdit(string id, InventoryChange model, string redirecturl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrEmpty(id))
				{
					_context.InventoryChanges.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirecturl);
				}

				_context.InventoryChanges.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirecturl);
			}

			return PartialView("AddEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var select = await _context.InventoryChanges.SingleOrDefaultAsync(i => i.Id == id);
			if (select == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("Delete", $"{select.Product?.Name} {select.Date}");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.InventoryChanges.FirstOrDefaultAsync(i => i.Id == id);

				_context.InventoryChanges.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}