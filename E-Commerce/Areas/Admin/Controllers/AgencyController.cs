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
	public class AgencyController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AgencyController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var query = await (from a in _context.Agencies
							   join c in _context.Cities on a.CityId equals c.Id

							   select new Agency
							   {
								   Id = a.Id,
								   Title = a.Title,
								   FullName = a.FullName,

								   CityId = a.CityId,
								   //CityName = c.Name,
								   Address = a.Address,
								   //Plaque = a.Plaque,
								   PostalCode = a.PostalCode,
								   Phone = a.Phone
							   }).ToListAsync();

			return View(query);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditAgancy(string id)
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
				await using (_context)
				{
					Agency agency = await _context.Agencies.Where(a => a.Id == id).SingleOrDefaultAsync();
					City city = await _context.Cities.Where(c => c.Id == agency.CityId).SingleOrDefaultAsync();
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

			return PartialView("AddEditAgancy", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditAgancy(string id, Agency model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					await using (_context)
					{
						_context.Agencies.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
				else
				{
					await using (_context)
					{
						_context.Agencies.Update(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
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

			return PartialView("AddEditAgancy", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteAgency(string id)
		{
			var agency = new Agency();
			await using (_context)
			{
				agency = await _context.Agencies.Where(a => a.Id == id).SingleOrDefaultAsync();
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
				await using (_context)
				{
					var agency = await _context.Agencies.Where(a => a.Id == id).SingleOrDefaultAsync();

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