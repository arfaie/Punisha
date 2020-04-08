using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _context;

		private readonly IWebHostEnvironment _env;

		public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
		{
			_context = context;

			_env = env;
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			ViewBag.rootpath = "/upload/thumbnailimage/";

			return View(await _context.Products.ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEdit(string id)
		{
			ViewBag.Cars = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name");

			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");

			ViewBag.Brands = new SelectList(await _context.Brands.ToListAsync(), "Id", "Title");

			ViewBag.Units = new SelectList(await _context.Units.ToListAsync(), "Id", "Title");

			var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
			if (product != null)
			{
				var carProducts = await _context.CarProducts.Where(x => x.ProductId == product.Id).ToListAsync();

				product.CarIds = carProducts.Select(x => x.CarId).ToArray();

				return PartialView("AddEdit", product);
			}

			return PartialView("AddEdit", new Product());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEdit(Product model, string imgName, string id, IEnumerable<IFormFile> files)
		{
			if (ModelState.IsValid)
			{
				//upload image
				var uploads = Path.Combine(_env.WebRootPath, "upload\\normalimage\\");

				foreach (var file in files)
				{
					if (file != null && file.Length > 0)
					{
						var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);

						await using (var fs = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
						{
							await file.CopyToAsync(fs);
							model.ImageName = filename;
						}
						var img = new ImageResizer();
						img.Resize(uploads + filename, _env.WebRootPath + "\\upload\\thumbnailimage\\" + filename);
					}
				}
				//upload image

				//var select = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

				if (String.IsNullOrWhiteSpace(id))
				{
					if (model.ImageName == null)
					{
						model.ImageName = "defaultpic.png";
					}

					//Add PriceChange
					//PriceChange priceChange = new PriceChange();
					//priceChange.ProductId = model.Id;
					//priceChange.Old = model.Price;

					//var CurrentDate = DateTime.Now;
					//PersianCalendar pcalender = new PersianCalendar();
					//int year = pcalender.GetYear(CurrentDate);
					//int month = pcalender.GetMonth(CurrentDate);
					//int day = pcalender.GetDayOfMonth(CurrentDate);
					//string ShamsiDate = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(year + "/" + month + "/" + day));

					//priceChange.Date = Convert.ToDateTime(ShamsiDate);
					//_context.PriceChanges.Add(priceChange);
					//Add PriceChange

					_context.Products.Add(model);

					await _context.SaveChangesAsync();

					var carProducts = await _context.CarProducts.Where(x => x.ProductId == model.Id).ToListAsync();
					_context.CarProducts.RemoveRange(carProducts);

					if (model.CarIds != null)
					{
						foreach (var carId in model.CarIds)
						{
							var newCarProduct = new CarProduct
							{
								CarId = carId,
								ProductId = model.Id
							};

							_context.CarProducts.Add(newCarProduct);
						}
					}

					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
					//return PartialView("_SuccessfulResponse", redirectUrl);
					//return Json(new { status = "success", message = "محصول با موفقیت ایجاد شد" });
					return RedirectToAction("Index");
				}

				if (model.ImageName == null)
				{
					model.ImageName = imgName;
				}

				//if (select.Price != model.Price)
				//{
				//    PriceChange priceChange = new PriceChange();
				//    priceChange.ProductId = model.Id;
				//    priceChange.Old = model.Price;

				//    var CurrentDate = DateTime.Now;
				//    PersianCalendar pcalender = new PersianCalendar();
				//    int year = pcalender.GetYear(CurrentDate);
				//    int month = pcalender.GetMonth(CurrentDate);
				//    int day = pcalender.GetDayOfMonth(CurrentDate);
				//    string ShamsiDate = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(year + "/" + month + "/" + day));

				//    priceChange.Date = Convert.ToDateTime(ShamsiDate);
				//    _context.PriceChanges.Add(priceChange);
				//}

				_context.CarProducts.RemoveRange(await _context.CarProducts.Where(x => x.ProductId == model.Id).ToListAsync());

				if (model.CarIds != null)
				{
					foreach (var carId in model.CarIds)
					{
						var newCarProduct = new CarProduct
						{
							CarId = carId,
							ProductId = model.Id
						};

						_context.CarProducts.Add(newCarProduct);
					}
				}

				_context.Products.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
				//return Json(new { status = "success", message = "اطلاعات محصول با موفقیت ویرایش شد" });
				//return PartialView("_SuccessfulResponse", redirectUrl);
				return RedirectToAction("Index");
			}
			if (!String.IsNullOrWhiteSpace(id))
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, ToastType.Yellow);
			}
			else
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, ToastType.Yellow);
			}

			ViewBag.Cars = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name");

			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");

			ViewBag.Brands = new SelectList(await _context.Brands.ToListAsync(), "Id", "Title");

			ViewBag.Units = new SelectList(await _context.Units.ToListAsync(), "Id", "Title");

			return PartialView("AddEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
			if (product == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("Delete", product.Name);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var product = await _context.Products.SingleOrDefaultAsync(a => a.Id == id);

				var sourcePath = Path.Combine(_env.WebRootPath, "upload\\normalimage\\" + product.ImageName);
				if (System.IO.File.Exists(sourcePath))
				{
					System.IO.File.Delete(sourcePath);
				}

				var sourcePath2 = Path.Combine(_env.WebRootPath, "upload\\thumbnailimage\\" + product.ImageName);
				if (System.IO.File.Exists(sourcePath2))
				{
					System.IO.File.Delete(sourcePath2);
				}

				var carProducts = await _context.CarProducts.Where(x => x.ProductId == id).ToListAsync();
				_context.CarProducts.RemoveRange(carProducts);

				_context.Products.Remove(product);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}
			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> GetProductField(string id)
		{
			var product = await _context.Products.SingleOrDefaultAsync(a => a.Id == id);
			var categoryId = product.CategoryId;

			//دسته بندی کالای ما شامل کدام گروه فیلد هاست؟انتخاب همه گروه فیلدها
			var selectGroupField = _context.CategoryFields.Where(a => a.CategoryId == categoryId);

			var selectProductField = _context.ProductFields.Where(x => x.ProductId == id);

			//کل فیلد ها در چند گروه اند؟انتخاب تمامی گروه ها بدون تکرار
			//List<int> lsFieldGroup = new List<int>();
			//foreach (var item in _context.Fields)
			//{
			//    if (!lsFieldGroup.Contains(item.FieldGroupId))
			//    {
			//        lsFieldGroup.Add(item.FieldGroupId);
			//    }
			//}

			//var model = (from f in _context.Fields
			//             join pg in SelectGroupField on f.Id equals pg.FieldId
			//             join pf in SelectProductField on f.Id equals pf.FieldId
			//             join psi in _context.ProductSelectedItems on pf.Id equals psi.ProductFieldId
			//             select new ProductField()
			//             {
			//                 ItemId = psi.ItemId,
			//                 CategoryId = pg.CategoryId,
			//                 FieldId = f.Id,
			//                 FieldGroupId = f.FieldGroupId,
			//                 Type = f.Type,
			//                 Title = f.Title,
			//                 lsFieldGroup = lsFieldGroup,
			//                 ProductId = pf.ProductId,
			//                 ProductFieldId = pf.Id,
			//                 Value = pf.Value,
			//                 //CarId=_context.ProductSelectedItems.Where(x=>x.ProductFieldId==pf.Id),
			//                 fieldGroup = _context.FieldGroups.Where(x => x.Id == f.FieldGroupId).FirstOrDefault().Title,
			//                 //CarListItems = new List<SelectListItem>(),

			//             }).ToList();

			//var ProductFieldId_ = model.Where(x => x.Type == 4).ToList();

			//var selectProductFieldId = model.Where(x => x.Type == 4).FirstOrDefault();
			//if (selectProductFieldId != null)
			//{
			//    string ProductFieldId = selectProductFieldId.ProductFieldId;
			//    var select = _context.ProductSelectedItems.Where(a => a.ProductFieldId == ProductFieldId);
			//    int[] b = new int[999];
			//    int i = 0;
			//    foreach (var item in select)
			//    {
			//        try
			//        {
			//            b[i] = item.ItemId;
			//        }
			//        catch (Exception e)
			//        {
			//            throw;
			//        }

			//        i++;
			//    }
			//    TempData["key"] = b;
			//}

			//try
			//{
			//    model.FirstOrDefault().CarListItems = await _context.Cars.Select(c => new SelectListItem
			//    {
			//        Text = c.Name,
			//        Value = c.Id.ToString()
			//    }).ToListAsync();
			//}
			//catch (Exception e)
			//{
			//}

			//var ss = (from f in _context.Fields
			//		  join fg in _context.FieldGroups on f.FieldGroupId equals fg.Id
			//		  join pcf in _context.CategoryFields on f.Id equals pcf.FieldId
			//		  join pf in _context.ProductFields on f.Id equals pf.FieldId
			//		  join p in _context.Products on pf.ProductId equals p.Id
			//		  join psi in _context.ProductSelectedItems on pf.Id equals psi.ProductFieldId into dep
			//		  from dept in dep.DefaultIfEmpty()
			//		  join c in _context.Cars on dept.ItemId equals c.Id into dep1
			//		  from dept1 in dep1.DefaultIfEmpty()
			//		  ).ToList()).ToList();

			var selectProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
			//TempData["lsFieldGroup"] = lsFieldGroup;

			TempData["CarListItems"] = await _context.Cars.Select(c => new SelectListItem
			{
				Text = c.Name,
				Value = c.Id.ToString()
			}).ToListAsync();
			return PartialView("ProductField");//ss
		}

		//public string CarNames(string id)
		//{
		//    if (!String.IsNullOrWhiteSpace(id))
		//    {
		//        var select = _context.ProductSelectedItems.Where(x => x.ProductFieldId == id);
		//        var cars = new List<string>();
		//        var i = 0;
		//        foreach (var item in select)
		//        {
		//            var name = _context.Cars.FirstOrDefault(x => x.Id == item.Id)?.Name;
		//            cars.Add(name);
		//        }
		//        return String.Join("،", cars);
		//    }
		//    return null;
		//}

		//public string[] CarIds(string productFieldId)
		//{
		//    if (!String.IsNullOrWhiteSpace(productFieldId))
		//    {
		//        var select = _context.ProductSelectedItems.Where(x => x.ProductFieldId == productFieldId);
		//        var ds = new string[select.Count()];
		//        var i = 0;
		//        foreach (var item in select)
		//        {
		//            try
		//            {
		//                ds[i] = item.Id;
		//                i++;
		//            }
		//            catch (Exception e)
		//            {
		//                throw;
		//            }
		//        }
		//        return ds;
		//    }
		//    return null;
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult AddEdit(Field model)
		//{
		//	var lsFieldId = (int[])TempData["lsFieldId"];
		//	string productId = (int)TempData["ProductId"];
		//	string productFieldId = (int)TempData["ProductFieldId"];
		//	var tp = model.FieldType;
		//	//check is Update
		//	var carIds = Request.Form["bb"];
		//	var select = _context.ProductSelectedItems.Where(x => x.ProductFieldId == productFieldId);
		//	Product selectedItems;

		//	if (model.CarIds != null)
		//	{
		//		if (select.Any())
		//		{
		//			_context.ProductSelectedItems.RemoveRange(select);
		//			foreach (var item in model.CarIds)
		//			{
		//				selectedItems = new Product();
		//				selectedItems.ItemId = Convert.ToInt16(item);
		//				selectedItems.ProductFieldId = productFieldId;
		//				_context.ProductSelectedItems.Add(selectedItems);
		//			}
		//		}
		//		else
		//		{
		//			foreach (var item in model.CarIds)
		//			{
		//				selectedItems = new ProductSelectedItems();
		//				selectedItems.ItemId = Convert.ToInt16(item);
		//				selectedItems.ProductFieldId = productFieldId;
		//				_context.ProductSelectedItems.Add(selectedItems);
		//			}
		//		}
		//	}

		//	foreach (var item in lsFieldId)
		//	{
		//		var selectPf = _context.ProductFields.Where(x => x.FieldId == item && x.ProductId == productId).FirstOrDefault();
		//		string value = Request.Form[item.ToString()];
		//		selectPf.Value = value;
		//		_context.ProductFields.Update(selectPf);
		//	}
		//	_context;
		//	_context.SaveChanges();

		//	TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

		//	return RedirectToAction("Index");
		//}
	}
}