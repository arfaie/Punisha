using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class SliderController : Controller
	{
		private readonly ApplicationDbContext _context;

		private readonly IWebHostEnvironment _env;

		public SliderController(ApplicationDbContext context, IWebHostEnvironment env)
		{
			_context = context;

			_env = env;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.Sliders.ToListAsync();
			ViewBag.rootpath = "/upload/thumbnailimage/";
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditSlider(string id)
		{
			Slider model = new Slider();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					Slider slid = _context.Sliders.Where(n => n.Id == id).SingleOrDefault();
					if (slid != null)
					{
						model.Id = slid.Id;
						model.Image = slid.Image;
					}
				}
			}

			return PartialView("AddEditSlider", model);
		}

		[HttpPost]
		public async Task<IActionResult> AddEditSlider(string id, Slider model, IEnumerable<IFormFile> files, string imgename)
		{
			if (ModelState.IsValid)
			{
				var uploud = Path.Combine(_env.WebRootPath, "upload\\normalimage\\");
				foreach (var file in files)
				{
					if (file != null && file.Length > 0)
					{
						var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
						using (var fs = new FileStream(Path.Combine(uploud, filename), FileMode.Create))
						{
							await file.CopyToAsync(fs);
							model.Image = filename;
						}
						ImageResizer image = new ImageResizer();
						image.Resize(uploud + filename, _env.WebRootPath + "\\upload\\thumbnailimage\\" + filename);
					}
				}

				if (!String.IsNullOrWhiteSpace(id))
				{
					if (model.Image == null)
					{
						model.Image = "defaultpic.png";
					}
					await using (_context)
					{
						_context.Sliders.Add(model);
						await _context.SaveChangesAsync();
					}
					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);
					return Json(new { Status = "success" });
				}
				else
				{
					if (model.Image == null)
					{
						model.Image = imgename;
					}
					await using (_context)
					{
						_context.Sliders.Update(model);
						await _context.SaveChangesAsync();
					}
					TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);
					return Json(new { Status = "success" });
				}
			}
			var list = new List<string>();
			foreach (var validation in ViewData.ModelState.Values)
			{
				list.AddRange(validation.Errors.Select(error => error.ErrorMessage));
			}
			return Json(new { Status = "error" });
		}

		[HttpGet]
		public async Task<IActionResult> DeleteSlider(string id)
		{
			var slider = new Slider();
			if (!String.IsNullOrWhiteSpace(id))
			{
				await using (_context)
				{
					slider = await _context.Sliders.Where(b => b.Id == id).SingleOrDefaultAsync();
					if (slider == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteSlider");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteSlider(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				await using (_context)
				{
					var model = await _context.Sliders.Where(b => b.Id == id).SingleOrDefaultAsync();

					string sourcePath = Path.Combine(_env.WebRootPath, "upload\\normalimage\\" + model.Image);
					if (System.IO.File.Exists(sourcePath))
					{
						System.IO.File.Delete(sourcePath);
					}

					string sourcePath2 = Path.Combine(_env.WebRootPath, "upload\\thumbnailimage\\" + model.Image);
					if (System.IO.File.Exists(sourcePath2))
					{
						System.IO.File.Delete(sourcePath2);
					}

					_context.Sliders.Remove(model);
					await _context.SaveChangesAsync();
				}
				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.Red);
				return PartialView("_SuccessfulResponse", redirectUrl);
			}
			else
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Red);
				return RedirectToAction("Index");
			}
		}
	}
}