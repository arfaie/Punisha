using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// TODO field
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

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
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

            return View(await _context.Fields.ToListAsync());
        }

        //public string FieldType(string id)
        //{
        //	var select = _context.FieldTypes.Where(x => x.Id == id).First().Title;
        //	return select;
        //}

        //public string ProductCtgNamesAsync(string FieldId)
        //{
        //	var titels = new List<string>();

        //	var select = _context.CategoryFields.Where(x => x.FieldId == FieldId).ToList();
        //	foreach (var item in select.ToList())
        //	{
        //		var title = _context.Categories.Where(x => x.Id == item.CategoryId).FirstOrDefault().Title;
        //		titels.Add(title);
        //	}

        //	return String.Join("،", titels);
        //}

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.SelectGroups = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
            ViewBag.FieldGroups = new SelectList(await _context.FieldGroups.ToListAsync(), "Id", "Title");
            ViewBag.FieldTypes = new SelectList(await _context.FieldTypes.ToListAsync(), "Id", "Title");

            var field = await _context.Fields.SingleOrDefaultAsync(b => b.Id == id);
            if (field != null)
            {
                return PartialView("AddEdit", field);
            }

            return PartialView("AddEdit", new Field());
        }
    }
}

//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> AddEdit(string id, Field model, string redirectUrl)
		//{
		//	if (ModelState.IsValid)
		//	{
		//              //var query = (from f in _context.Fields
		//              //			 join pcf in _context.CategoryFields on f.Id equals pcf.FieldId).Where(x => x.Type == 4);
		//              if (Id == 0)
		//              {
		//                  using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
		//                  {
		//                      //model.Type = (short)model.SelecteFieldTypeId;
		//                      int[] aa = model.;
		//                      if (model.IdSelectGroup == 0)
		//                          model.IdSelectGroup = -1;
		//                      Field fieldModel = _mapper.Map<AddEditFieldViewModel, Field>(model);

		//                      if (!CheckDuplicateDrp(model.IdCategory) || model.Type != 4)
		//                      {
		//                          db.Fields.Add(fieldModel);
		//                          await db.SaveChangesAsync();
		//                          var select = _context.Fields.LastOrDefault();
		//                          int ss = select.Id;

		//                          foreach (var item in aa)
		//                          {
		//                              ProductCategoryFields groupField = new ProductCategoryFields();
		//                              groupField.IdFaild = ss;
		//                              groupField.IdCategory = item;
		//                              db.ProductCategoryFields.Add(groupField);

		//                              //Add to ProductFields
		//                              ProductField pf = new ProductField();
		//                              var selectProducts = _context.Products.Where(x => x.IdCategory == item);
		//                              foreach (var itemProduct in selectProducts)
		//                              {
		//                                  pf.IdProduct = itemProduct.Id;
		//                                  pf.IdField = ss;
		//                                  pf.Value = null;
		//                                  db.ProductFields.Add(pf);
		//                              }
		//                              //Add to ProductFields
		//                          }

		//                          await db.SaveChangesAsync();
		//                      }
		//                      else
		//                      {
		//                          TempData["Notif"] = Notification.ShowNotif("فیلد خودرد ها قبلا به این دسته بندی اضافه شده است", type: ToastType.red);

		//                          return PartialView("_Succefullyresponse", redirectUrl);
		//                      }
		//                  }

		//                  TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);
		//                  return PartialView("_Succefullyresponse", redirectUrl);
		//              }

		//              //{
		//              //
		//              //	{
		//              //		_context.CategoryFields.RemoveRange(_context.CategoryFields.Where(x => x.FieldId == id));

		//              //		if (model.ICategories != null)
		//              //		{
		//              //		}
		//              //		await _context.SaveChangesAsync();
		//              //		if (!CheckDuplicateDrp(model.CategoryId))
		//              //		{
		//              //			var selectType = _context.Fields.AsNoTracking().Where(x => x.Id == id).AsNoTracking().FirstOrDefault();

		//              //			if (selectType.Type == 4 && model.Type != 4)
		//              //			{
		//              //				string ProductFieldId = _context.ProductFields.Where(x => x.FieldId == selectType.Id).AsNoTracking().FirstOrDefault().Id;
		//              //				_context.ProductSelectedItems.RemoveRange(_context.ProductSelectedItems.Where(x => x.ProductFieldId == ProductFieldId));
		//              //				//await _context.SaveChangesAsync();
		//              //			}
		//              //			model.Type = (short)model.Type;
		//              //			if (model.SelectGroupId == 0)
		//              //				model.SelectGroupId = -1;
		//              //			int[] aa = model.CategoryId;
		//              //			Field fieldModel = AutoMapper.Mapper.Map<Field, Field>(model);
		//              //			try
		//              //			{
		//              //				_context.Fields.Update(fieldModel);
		//              //			}
		//              //			catch (Exception e)
		//              //			{
		//              //				throw;
		//              //			}
		//              //			foreach (var item in aa)
		//              //			{
		//              //				CategoryFields groupField = new CategoryFields();
		//              //				groupField.FieldId = id;
		//              //				groupField.CategoryId = item;
		//              //				_context.CategoryFields.Add(groupField);
		//              //			}
		//              //			await _context.SaveChangesAsync();
		//              //		}
		//              //		else
		//              //		{
		//              //			TempData["Notification"] = Notification.ShowNotif("فیلد خودرد ها قبلا به این دسته بندی اضافه شده است", type: ToastType.Red);
		//              //			return PartialView("_SuccessfulResponse", redirectUrl);
		//              //		}
		//              //	}

		//              //	TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.Blue);

		//              //	return PartialView("_SuccessfulResponse", redirectUrl);
		//              //}
		//          }

		//	ViewBag.SelectGroups = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
		//	ViewBag.FieldGroups = new SelectList(await _context.FieldGroups.ToListAsync(), "Id", "Title");
		//	ViewBag.FieldTypes = new SelectList(await _context.FieldTypes.ToListAsync(), "Id", "Title");

		//	return PartialView("AddEdit", model);
		//}

		//public bool CheckDuplicateDrp(int[] idCat)
		//{
		//	var exist = true;
		//	//var query = (from f in _context.Fields
		//	//	join pcf in _context.CategoryFields on f.Id equals pcf.FieldId).Where(x => x.Type == 4);
		//	//foreach (var item in idCat)
		//	//{
		//	//	exist = query.Where(x => x.CategoryId == item).Any();//آیتمی وجود دارد؟
		//	//	if (exist == true)
		//	//	{
		//	//		break;
		//	//	}
		//	//}
		//	return exist;
		//}

		//[HttpGet]
		//[AutoValidateAntiforgeryToken]
		//public async Task<IActionResult> Delete(string id)
		//{
		//	var field = new Field();

		//	{
		//		field = await _context.Fields.SingleOrDefaultAsync(a => a.Id == id);
		//		if (field == null)
		//		{
		//			return RedirectToAction("Index");
		//		}
		//	}

		//	return PartialView("Delete", field.Title);
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Delete(string id, string redirectUrl)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		{
		//			var field = await _context.Fields.SingleOrDefaultAsync(a => a.Id == id);

		//			_context.Fields.Remove(field);
		//			await _context.SaveChangesAsync();

		//			TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
		//			return PartialView("_SuccessfulResponse", redirectUrl);
		//		}
		//	}

		//	TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

		//	return RedirectToAction("Index");
//		//}
	}
}