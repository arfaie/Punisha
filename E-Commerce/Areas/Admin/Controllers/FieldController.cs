using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class FieldController : Controller
	{
		private readonly ApplicationDbContext _context;

		public FieldController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			//var ff = _context.Fields.Where(pr).ToList();
			//var query = (from field in _context.Fields
			//			 join cf in _context.CategoryFields on field.Id equals cf.FieldId
			//			 select new FieldViewModel
			//			 {
			//				 Id = field.Id,
			//				 Title = field.Title,
			//				 IdType = field.Type,
			//				 SelectGroupName = "",
			//				 FieldGroupId = field.FieldGroupId,
			//				 SelectFieldGroupName = ""
			//			 }).GroupBy(x => x.Id);
			//List<FieldViewModel> lsFieldViewModel = new List<FieldViewModel>();
			//FieldViewModel ob;
			//foreach (var item in query)
			//{
			//	ob = new FieldViewModel();
			//	ob.Id = item.First().Id;
			//	ob.Title = item.First().Title;
			//	ob.Type = FieldType(item.First().IdType);
			//	ob.SelectGroupName = "";
			//	ob.FieldGroupId = item.First().FieldGroupId;
			//	ob.SelectFieldGroupName = ProductCtgNamesAsync(item.First().Id);
			//	lsFieldViewModel.Add(ob);
			//}

			return View();
		}

		public string FieldType(string id)
		{
			var select = _context.FieldTypes.Where(x => x.Id == id).First().Title;
			return select;
		}

		public string ProductCtgNamesAsync(string FieldId)
		{
			var titels = new List<string>();

			var select = _context.CategoryFields.Where(x => x.FieldId == FieldId).ToList();
			foreach (var item in select.ToList())
			{
				var title = _context.Categories.Where(x => x.Id == item.CategoryId).FirstOrDefault().Title;
				titels.Add(title);
			}

			return String.Join("،", titels);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditField(string id)
		{
			var model = new Field();
			//model.SelectGroupList = await _context.SelectGroups.ToListAsync();

			//model.ICategories = await _context.Categories.Select(s => new SelectListItem
			//{
			//	Text = s.Title,
			//	Value = s.Id.ToString()
			//}).ToListAsync();
			//model.SelectFiledGroupList = await _context.FieldGroups.ToListAsync();
			//model.FildTypeModels = await _context.FieldTypes.Where(x => x.Id < 5).ToListAsync();

			if (!String.IsNullOrWhiteSpace(id))
			{
				{
					var field = await _context.Fields.SingleOrDefaultAsync(a => a.Id == id);
					//var select = _context.CategoryFields.Where(a => a.FieldId == id);
					//int[] b = new int[999];
					//int i = 0;
					//foreach (var item in select)
					//{
					//	try
					//	{
					//		b[i] = item.CategoryId;
					//	}
					//	catch (Exception e)
					//	{
					//		throw;
					//	}

					//	i++;
					//}
					//if (field != null)
					//{
					//	model.Id = field.Id;
					//	model.Title = field.Title;
					//	model.Type = field.Type;
					//	model.SelectGroupId = field.SelectGroupId;
					//	model.FieldGroupId = field.FieldGroupId;
					//	model.CategoryId = b;
					//}
				}
			}

			return PartialView("AddEditField", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditField(string id, Field model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				//var query = (from f in _context.Fields
				//			 join pcf in _context.CategoryFields on f.Id equals pcf.FieldId).Where(x => x.Type == 4);
				//if (!String.IsNullOrWhiteSpace(id))
				//{
				//
				//	{
				//		//model.Type = (short)model.SelecteFieldTypeId;
				//		int[] aa = model.CategoryId;
				//		if (model.SelectGroupId == 0)
				//			model.SelectGroupId = -1;
				//		Field fieldModel = AutoMapper.Mapper.Map<Field, Field>(model);

				//		if (!CheckDuplicateDrp(model.CategoryId) || model.Type != 4)
				//		{
				//			_context.Fields.Add(fieldModel);
				//			await _context.SaveChangesAsync();
				//			var select = _context.Fields.LastOrDefault();
				//			int ss = select.Id;

				//			foreach (var item in aa)
				//			{
				//				CategoryFields groupField = new CategoryFields();
				//				groupField.FieldId = ss;
				//				groupField.CategoryId = item;
				//				_context.CategoryFields.Add(groupField);

				//				//Add to ProductFields
				//				ProductField pf = new ProductField();
				//				var selectProducts = _context.Products.Where(x => x.CategoryId == item);
				//				foreach (var itemProduct in selectProducts)
				//				{
				//					pf.ProductId = itemProduct.Id;
				//					pf.FieldId = ss;
				//					pf.Value = null;
				//					_context.ProductFields.Add(pf);
				//				}
				//				//Add to ProductFields
				//			}

				//			await _context.SaveChangesAsync();
				//		}
				//		else
				//		{
				//			TempData["Notification"] = Notification.ShowNotif("فیلد خودرد ها قبلا به این دسته بندی اضافه شده است", type: ToastType.Red);

				//			return PartialView("_SuccessfulResponse", redirectUrl);
				//		}
				//	}

				//{
				//
				//	{
				//		_context.CategoryFields.RemoveRange(_context.CategoryFields.Where(x => x.FieldId == id));

				//		if (model.ICategories != null)
				//		{
				//		}
				//		await _context.SaveChangesAsync();
				//		if (!CheckDuplicateDrp(model.CategoryId))
				//		{
				//			var selectType = _context.Fields.AsNoTracking().Where(x => x.Id == id).AsNoTracking().FirstOrDefault();

				//			if (selectType.Type == 4 && model.Type != 4)
				//			{
				//				string ProductFieldId = _context.ProductFields.Where(x => x.FieldId == selectType.Id).AsNoTracking().FirstOrDefault().Id;
				//				_context.ProductSelectedItems.RemoveRange(_context.ProductSelectedItems.Where(x => x.ProductFieldId == ProductFieldId));
				//				//await _context.SaveChangesAsync();
				//			}
				//			model.Type = (short)model.Type;
				//			if (model.SelectGroupId == 0)
				//				model.SelectGroupId = -1;
				//			int[] aa = model.CategoryId;
				//			Field fieldModel = AutoMapper.Mapper.Map<Field, Field>(model);
				//			try
				//			{
				//				_context.Fields.Update(fieldModel);
				//			}
				//			catch (Exception e)
				//			{
				//				throw;
				//			}
				//			foreach (var item in aa)
				//			{
				//				CategoryFields groupField = new CategoryFields();
				//				groupField.FieldId = id;
				//				groupField.CategoryId = item;
				//				_context.CategoryFields.Add(groupField);
				//			}
				//			await _context.SaveChangesAsync();
				//		}
				//		else
				//		{
				//			TempData["Notification"] = Notification.ShowNotif("فیلد خودرد ها قبلا به این دسته بندی اضافه شده است", type: ToastType.Red);
				//			return PartialView("_SuccessfulResponse", redirectUrl);
				//		}
				//	}

				//	TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

				//	return PartialView("_SuccessfulResponse", redirectUrl);
				//}
			}

			if (!String.IsNullOrWhiteSpace(id))
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, type: ToastType.Yellow);
			}
			else
			{
				TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, type: ToastType.Yellow);
			}

			//model.SelectGroupList = await _context.SelectGroups.ToListAsync();
			//model.SelectGroupList = await _context.SelectGroups.Select(c => new SelectListItem()
			//{
			//    Text = c.Title,
			//    Value = c.Id.ToString()
			//}).ToListAsync();

			return PartialView("AddEditField", model);
		}

		public bool CheckDuplicateDrp(int[] idCat)
		{
			var exist = true;
			//var query = (from f in _context.Fields
			//	join pcf in _context.CategoryFields on f.Id equals pcf.FieldId).Where(x => x.Type == 4);
			//foreach (var item in idCat)
			//{
			//	exist = query.Where(x => x.CategoryId == item).Any();//آیتمی وجود دارد؟
			//	if (exist == true)
			//	{
			//		break;
			//	}
			//}
			return exist;
		}

		[HttpGet]
		public async Task<IActionResult> DeleteField(string id)
		{
			var field = new Field();

			{
				field = await _context.Fields.SingleOrDefaultAsync(a => a.Id == id);
				if (field == null)
				{
					return RedirectToAction("Index");
				}
			}

			return PartialView("DeleteField", field.Title);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteField(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				{
					var field = await _context.Fields.SingleOrDefaultAsync(a => a.Id == id);

					_context.Fields.Remove(field);
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