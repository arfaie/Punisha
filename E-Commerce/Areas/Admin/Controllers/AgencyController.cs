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
	public class AgencyController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AgencyController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			ViewBag.Cities = new SelectList(await _context.Cities.ToListAsync());

			return View(await _context.Agencies.ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> AddEditAgency(string id)
		{
			var model = new Agency();
			//model.CitiesList = await _context.Cities.Select(c => new SelectListItem
			//{
			//	Text = c.Name,
			//	Value = c.Id.ToString()
			//}).ToListAsync();

			//model.StatesList = await _context.States.Select(s => new SelectListItem
			//{
			//	Text = s.Name,
			//	Value = s.Id.ToString()
			//}).ToListAsync();

			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					var agency = await _context.Agencies.SingleOrDefaultAsync(a => a.Id == id);
					var city = await _context.Cities.SingleOrDefaultAsync(c => c.Id == agency.CityId);
					if (agency != null)
					{
						model.Id = agency.Id;
						model.Title = agency.Title;
						model.FullName = agency.FullName;
						model.CityId = agency.CityId;
						model.Address = agency.Address;
						model.PostalCode = agency.PostalCode;
						model.Phone = agency.Phone;
					}
				}
			}

			return PartialView("AddEditAgency", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditAgency(string id, Agency model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					{
						_context.Agencies.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				{
					_context.Agencies.Update(model);
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

			//model.CitiesList = await _context.Cities.Select(c => new SelectListItem
			//{
			//	Text = c.Name,
			//	Value = c.Id.ToString()
			//}).ToListAsync();

			//model.StatesList = await _context.States.Select(s => new SelectListItem
			//{
			//	Text = s.Name,
			//	Value = s.Id.ToString()
			//}).ToListAsync();

			return PartialView("AddEditAgency", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteAgency(string id)
		{
			var agency = new Agency();

			{
				agency = await _context.Agencies.SingleOrDefaultAsync(a => a.Id == id);
				if (agency == null)
				{
					return RedirectToAction("Index");
				}
			}

			return PartialView("DeleteAgency", agency.Title);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAgency(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				{
					var agency = await _context.Agencies.SingleOrDefaultAsync(a => a.Id == id);

					_context.Agencies.Remove(agency);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}