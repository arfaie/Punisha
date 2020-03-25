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
	public class CarController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CarController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.Cars.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditCar(string id)
		{
			var car = new Car();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					car = await _context.Cars.FirstOrDefaultAsync(c => c.Id.ToString() == id);
					if (car == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("AddEditCar", car);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditCar(string id, Car model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					await using (_context)
					{
						_context.Cars.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
				else
				{
					await using (_context)
					{
						_context.Cars.Update(model);
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

				return PartialView("AddEditCar", model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> DeleteCar(string id)
		{
			var car = new Car();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					car = await _context.Cars.FirstOrDefaultAsync(c => c.Id.ToString() == id);
					if (car == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteCar", car.Name);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCar(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				await using (_context)
				{
					var model = await _context.Cars.FirstOrDefaultAsync(c => c.Id.ToString() == id);
					_context.Cars.Remove(model);
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