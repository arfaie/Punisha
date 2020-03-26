using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CityController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CityController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			//var model = await _context.Cities.ToListAsync();
			var model = await (from c in _context.Cities
							   join s in _context.States on c.StateId equals s.Id
							   select new City
							   {
								   Id = c.Id,
								   Name = c.Name,
								   StateId = c.StateId,
								   State = s
							   }).ToListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditCity(string id)
		{
			var model = new City();
			//model.StaSelectListItems = await _context.States.Select(s => new SelectListItem
			//{
			//	Text = s.Name,
			//	Value = s.Id.ToString()
			//}).ToListAsync();

			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
					if (city != null)
					{
						model.Id = city.Id;
						model.Name = city.Name;
						model.StateId = city.StateId;
					}
				}
			}

			return PartialView("AddEditCity", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditCity(string id, City city, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					{
						_context.Cities.Add(city);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				{
					_context.Cities.Update(city);
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

			//city.StaSelectListItems = await _context.States.Select(s => new SelectListItem
			//{
			//	Text = s.Name,
			//	Value = s.Id.ToString()
			//}).ToListAsync();

			return PartialView("AddEditCity", city);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteCity(string id)
		{
			var city = new City();
			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
					if (city == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteCity", city.Name);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCity(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				{
					var model = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
					_context.Cities.Remove(model);
					await _context.SaveChangesAsync();
				}
				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}