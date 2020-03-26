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
	public class ProductGalleryController : Controller
	{
		private readonly ApplicationDbContext _context;

		private readonly IWebHostEnvironment _env;

		public ProductGalleryController(ApplicationDbContext context, IWebHostEnvironment env)
		{
			_context = context;

			_env = env;
		}

		public IActionResult Index(string id)
		{
			if (!String.IsNullOrWhiteSpace(id))
			{
				return RedirectToAction("Index", "Product");
			}

			var model = _context.ProductGalleries.Where(g => g.ProductId == id).ToList();

			ViewBag.rootpath = "/upload/thumbnailimage/";
			//HttpContext.Session.SetInt32("ImageProductKey", id);

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> ProductGallery(string id)
		{
			var model = new ProductGallery();

			//model.ProductListItems = _context.Products.Select(a => new SelectListItem
			//{
			//	Text = a.Name,
			//	Value = a.Id.ToString()
			//}).ToList();

			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					var image = _context.ProductGalleries.Where(n => n.Id == id).SingleOrDefault();

					if (image != null)
					{
						model.Id = image.Id;
						model.ProductId = image.ProductId;
						model.Image = image.Image;
					}
				}
			}
			return PartialView("ProductGallery", model);
		}

		[HttpPost]
		public async Task<IActionResult> ProductGallery(ProductGallery model, string id, IEnumerable<IFormFile> files)
		{
			if (ModelState.IsValid)
			{
				var stList = new List<string>();

				var upload = Path.Combine(_env.WebRootPath, "upload\\normalimage\\");
				foreach (var faile in files)
				{
					if (faile != null && faile.Length > 0)
					{
						var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(faile.FileName);

						using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
						{
							await faile.CopyToAsync(fs);
							stList.Add(filename);
							//model.Image = filename;
						}

						var img = new ImageResizer();
						img.Resize(upload + filename, _env.WebRootPath + "\\upload\\thumbnailimage\\" + filename);
					}
				}
				if (model.Image == null)
				{
					model.Image = "defaultpic.png";
				}
				if (!String.IsNullOrWhiteSpace(id))
				{
					{
						foreach (var variable in stList)
						{
							model.Image = variable;
							//model.ProductId = (int)HttpContext.Session.GetInt32("ImageProductKey");
							_context.ProductGalleries.Add(model);
							_context.SaveChanges();
						}
					}
					return Json(new { Status = "success", Message = "عکس با موفقیت ثبت شد" });
				}

				{
					foreach (var variable in stList)
					{
						model.Image = variable;
						//model.ProductId = (int)HttpContext.Session.GetInt32("ImageProductKey");
						//ProductGallery imagemodel = AutoMapper.Mapper.Map<ProductGallery, ProductGallery>(model);
						_context.ProductGalleries.Update(model);
						_context.SaveChanges();
					}
				}
				return Json(new { Status = "success", Message = "عکس با موفقیت ویرایش شد" });
			}
			//model.ProductListItems = _context.Products.Select(a => new SelectListItem
			//{
			//	Text = a.Name,
			//	Value = a.Id.ToString()
			//}).ToList();

			var list = new List<string>();
			foreach (var validation in ViewData.ModelState.Values)
			{
				list.AddRange(validation.Errors.Select(error => error.ErrorMessage));
			}

			return Json(new { Status = "error", error = list });
		}

		[HttpGet]
		public async Task<IActionResult> DeleteProductGallery(string id)
		{
			var productGallery = new ProductGallery();
			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					productGallery = await _context.ProductGalleries.SingleOrDefaultAsync(b => b.Id == id);
					if (productGallery == null)
					{
						return RedirectToAction("Index");
					}
				}
			}

			return PartialView("DeleteProductGallery");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteProductGallery(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				{
					var model = await _context.ProductGalleries.SingleOrDefaultAsync(b => b.Id == id);

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

					_context.ProductGalleries.Remove(model);
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