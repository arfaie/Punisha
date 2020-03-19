﻿using Ecommerce.Models;
using Ecommerce.Models.Helpers;
using Ecommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CarController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceProvider _serviceProvider;

		public CarController(ApplicationDbContext context, IServiceProvider serviceProvider)
		{
			_context = context;
			_serviceProvider = serviceProvider;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.Cars.ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditCar(int Id)
		{
			var Car = new Car();
			if (Id != 0)
			{
				using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
				{
					Car = await _context.Cars.Where(c => c.Id == Id).SingleOrDefaultAsync();
					if (Car == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("AddEditCar", Car);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditCar(int Id, Car model, string redirecturl)
		{
			if (ModelState.IsValid)
			{
				if (Id == 0)
				{
					using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
					{
						db.Cars.Add(model);
						await db.SaveChangesAsync();
					}

					TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

					return PartialView("_Succefullyresponse", redirecturl);
				}
				else
				{
					using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
					{
						db.Cars.Update(model);
						await db.SaveChangesAsync();
					}

					TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);

					return PartialView("_Succefullyresponse", redirecturl);
				}
			}
			else
			{
				if (Id == 0)
				{
					TempData["Notif"] = Notification.ShowNotif(MessageType.addError, type: ToastType.yellow);
				}
				else
				{
					TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
				}

				return PartialView("AddEditCar", model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> DeleteCar(int Id)
		{
			var Car = new Car();
			if (Id != 0)
			{
				using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
				{
					Car = await _context.Cars.Where(c => c.Id == Id).SingleOrDefaultAsync();
					if (Car == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteCar", Car.Name);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCar(int id, string redirecturl)
		{
			if (ModelState.IsValid)
			{
				using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
				{
					var model = await db.Cars.Where(c => c.Id == id).SingleOrDefaultAsync();
					db.Cars.Remove(model);
					await db.SaveChangesAsync();
				}
				TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);

				return PartialView("_Succefullyresponse", redirecturl);
			}
			else
			{
				TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.yellow);

				return RedirectToAction("Index");
			}
		}
	}
}