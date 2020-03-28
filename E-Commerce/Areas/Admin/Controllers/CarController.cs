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
	public class CarController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CarController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Cars.Include(x => x.Maker).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEditCar(string id)
		{
			ViewBag.Makers = new SelectList(await _context.Makers.ToListAsync(), "Id", "Name");

			var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
			if (car != null)
			{
				return PartialView("AddEditCar", car);
			}

			return PartialView("AddEditCar", new Car());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditCar(string id, Car model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.Cars.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.Cars.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.Makers = new SelectList(await _context.Makers.ToListAsync(), "Id", "Name");

			return PartialView("AddEditCar", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> DeleteCar(string id)
		{
			var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
			if (car == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("DeleteCar", car.Name);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCar(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);

				_context.Cars.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}