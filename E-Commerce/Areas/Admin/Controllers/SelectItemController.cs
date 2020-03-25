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
	public class SelectItemController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SelectItemController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var query = await _context.SelectItems.ToListAsync();

			return View(query);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditSelectItem(string id)
		{
			SelectItem model = new SelectItem();
			//model.SelectGroupListItems = await _context.SelectGroups.Select(c => new SelectListItem
			//{
			//	Text = c.Title,
			//	Value = c.Id.ToString()
			//}).ToListAsync();

			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					SelectItem selectItem = await _context.SelectItems.Where(a => a.Id == id).SingleOrDefaultAsync();
					if (selectItem != null)
					{
						model.Id = selectItem.Id;
						model.Title = selectItem.Title;
						model.SelectGroupId = selectItem.SelectGroupId;
					}
				}
			}

			return PartialView("AddEditSelectItem", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditSelectItem(string id, SelectItem model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (!String.IsNullOrWhiteSpace(id))
				{
					await using (_context)
					{
						_context.SelectItems.Add(model);
						await _context.SaveChangesAsync();
					}

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}
				else
				{
					await using (_context)
					{
						_context.SelectItems.Update(model);
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

			//model.SelectGroupListItems = await _context.SelectGroups.Select(c => new SelectListItem
			//{
			//	Text = c.Title,
			//	Value = c.Id.ToString()
			//}).ToListAsync();

			return PartialView("AddEditSelectItem", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteSelectItem(string id)
		{
			var selectItem = new SelectItem();
			await using (_context)
			{
				selectItem = await _context.SelectItems.Where(a => a.Id == id).SingleOrDefaultAsync();
				if (selectItem == null)
				{
					return RedirectToAction("Index");
				}
			}

			return PartialView("DeleteSelectItem", selectItem.Title);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteSelectItem(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				await using (_context)
				{
					var selectItem = await _context.SelectItems.Where(a => a.Id == id).SingleOrDefaultAsync();

					_context.SelectItems.Remove(selectItem);
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