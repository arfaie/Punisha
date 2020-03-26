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
	public class BrandController : Controller
	{
		private readonly ApplicationDbContext _context;

		public BrandController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.Brands.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditBrand(string id)
		{
			var brand = new Brand();
			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					brand = await _context.Brands.SingleOrDefaultAsync(b => b.Id == id);
					if (brand == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("AddEditBrand", brand);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditBrand(string id, Brand model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					{
						_context.Brands.Add(model);
						await _context.SaveChangesAsync();
					}
					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);
					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				{
					_context.Brands.Update(model);
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

			return PartialView("AddEditBrand", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteBrand(string id)
		{
			var brand = new Brand();
			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					brand = await _context.Brands.SingleOrDefaultAsync(b => b.Id == id);
					if (brand == null)
					{
						return RedirectToAction("Index");
					}
				}
			}
			return PartialView("DeleteBrand", brand.Title);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteBrand(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				{
					var model = await _context.Brands.SingleOrDefaultAsync(b => b.Id == id);
					_context.Brands.Remove(model);
					await _context.SaveChangesAsync();
				}
				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Red);
			return RedirectToAction("Index");
		}
	}
}