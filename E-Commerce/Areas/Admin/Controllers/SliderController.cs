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
			var model = new Slider();
			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					var slid = _context.Sliders.Where(n => n.Id == id).SingleOrDefault();
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
						var image = new ImageResizer();
						image.Resize(uploud + filename, _env.WebRootPath + "\\upload\\thumbnailimage\\" + filename);
					}
				}

				if (!String.IsNullOrWhiteSpace(id))
				{
					if (model.Image == null)
					{
						model.Image = "defaultpic.png";
					}

					{
						_context.Sliders.Add(model);
						await _context.SaveChangesAsync();
					}
					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, type: ToastType.Green);
					return Json(new { Status = "success" });
				}

				if (model.Image == null)
				{
					model.Image = imgename;
				}

				{
					_context.Sliders.Update(model);
					await _context.SaveChangesAsync();
				}
				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);
				return Json(new { Status = "success" });
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
				{
					slider = await _context.Sliders.SingleOrDefaultAsync(b => b.Id == id);
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
				{
					var model = await _context.Sliders.SingleOrDefaultAsync(b => b.Id == id);

					var sourcePath = Path.Combine(_env.WebRootPath, "upload\\normalimage\\" + model.Image);
					if (System.IO.File.Exists(sourcePath))
					{
						System.IO.File.Delete(sourcePath);
					}

					var sourcePath2 = Path.Combine(_env.WebRootPath, "upload\\thumbnailimage\\" + model.Image);
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

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, type: ToastType.Red);
			return RedirectToAction("Index");
		}
	}
}