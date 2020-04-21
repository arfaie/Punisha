using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            //var query = (from field in _context.Fields.ToListAsync()
            //             join cf in _context.CategoryFields on field.Id equals cf.FieldId
            //             select new FieldViewModel
            //             {
            //                 Id = field.Id,
            //                 Title = field.Title,
            //                 IdType = field.Type,
            //                 SelectGroupName = "",
            //                 FieldGroupId = field.FieldGroupId,
            //                 SelectFieldGroupName = ""
            //             }).GroupBy(x => x.Id);
            //List<FieldViewModel> lsFieldViewModel = new List<FieldViewModel>();
            //FieldViewModel ob;
            //foreach (var item in query)
            //{
            //    ob = new FieldViewModel();
            //    ob.Id = item.First().Id;
            //    ob.Title = item.First().Title;
            //    ob.Type = FieldType(item.First().IdType);
            //    ob.SelectGroupName = "";
            //    ob.FieldGroupId = item.First().FieldGroupId;
            //    ob.SelectFieldGroupName = ProductCtgNamesAsync(item.First().Id);
            //    lsFieldViewModel.Add(ob);
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
			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.FieldGroups = new SelectList(await _context.FieldGroups.ToListAsync(), "Id", "Title");
			ViewBag.FieldTypes = new SelectList(await _context.FieldTypes.ToListAsync(), "Id", "Title");

			//var field = await _context.Fields.SingleOrDefaultAsync(b => b.Id == id);
			//if (field != null)
			//{
			//	return PartialView("AddEdit", field);
			//}

			return PartialView("AddEdit", new Field());
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, Field model, string redirectUrl,string[] CategoryId)
        {
            if (ModelState.IsValid)
            {
                if (!CheckDuplicateDrp(CategoryId) || model.FieldTypeId != "4")
                {
                    _context.Fields.Add(model);
                    await _context.SaveChangesAsync();
                    try
                    {
                        var select1 = _context.Fields;
                    }
                    catch (System.Exception e)
                    {

                        throw;
                    }
                    var select = _context.Fields;
                    string ss = select.ToList().Last().Id;

                    foreach (var item in CategoryId)
                    {
                        CategoryField CategoryField = new CategoryField();
                        CategoryField.FieldId = ss;
                        CategoryField.CategoryId = item;
                        _context.CategoryFields.Add(CategoryField);

                        //Add to ProductFields
                        ProductField pf = new ProductField();
                        var selectProducts = _context.Products.Where(x => x.CategoryId == item);
                        foreach (var itemProduct in selectProducts)
                        {
                            pf.ProductId = itemProduct.Id;
                            pf.FieldId = ss;
                            pf.Value = null;
                            _context.ProductFields.Add(pf);
                        }
                        //Add to ProductFields
                    }

                    await _context.SaveChangesAsync();
                }
            }

            ViewBag.SelectGroups = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
            ViewBag.FieldGroups = new SelectList(await _context.FieldGroups.ToListAsync(), "Id", "Title");
            ViewBag.FieldTypes = new SelectList(await _context.FieldTypes.ToListAsync(), "Id", "Title");

            return View("Index", model);
        }

        public bool CheckDuplicateDrp(string[] idCat)
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