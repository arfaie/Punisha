using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcApplication1.Models;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FieldController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationDbContext __context;
        private readonly IServiceProvider _serviceProvider;

        public FieldController(ApplicationDbContext context_, ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            __context = context_;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            List<int> ls = new List<int>();
            ls.Add(1);
            ls.Add(2);
            ls.Add(7);
            //var predicate = PredicateBuilder.True<Field>();

            var pr = PredicateBuilder.False<Field>();
            //IQueryable<Field> query1 = _context.Fields;
            foreach (var name in ls)
            {
                pr = pr.Or(x => x.Id == name);
            }
            foreach (var name in ls)
            {
                pr = pr.And(x => x.Type == 2);
            }
            var ff = _context.Fields.Where(pr).ToList();
            var query = (from field in _context.Fields
                         join CF in _context.ProductCategoryFields on field.Id equals CF.IdFaild
                         select new FieldViewModel()
                         {
                             Id = field.Id,
                             Title = field.Title,
                             IdType = field.Type,
                             SelectGroupName = "",
                             IdFieldGroup = field.IdFieldGroup,
                             SelectFieldGroupName = ""
                         }).GroupBy(x => x.Id);
            List<FieldViewModel> lsFieldViewModel = new List<FieldViewModel>();
            FieldViewModel ob;
            foreach (var item in query)
            {
                ob = new FieldViewModel();
                ob.Id = item.First().Id;
                ob.Title = item.First().Title;
                ob.Type = FieldType(item.First().IdType);
                ob.SelectGroupName = "";
                ob.IdFieldGroup = item.First().IdFieldGroup;
                ob.SelectFieldGroupName = ProductCtgNamesAsync(item.First().Id);
                lsFieldViewModel.Add(ob);
            }

            return View(lsFieldViewModel);

        }

        public string FieldType(int id)
        {

            var select = _context.FieldTypes.Where(x => x.Id == id).First().Title;
            return select;
        }
        public string ProductCtgNamesAsync(int idField)
        {
            List<string> Titels = new List<string>();

            var select = __context.ProductCategoryFields.Where(x => x.IdFaild == idField).ToList();
            foreach (var item in select.ToList())
            {
                string Title = __context.Categories.Where(x => x.Id == item.IdCategory).FirstOrDefault().Title;
                Titels.Add(Title);
            }

            return String.Join("،", Titels);

        }


        [HttpGet]
        public async Task<IActionResult> AddEditField(int id)
        {

            AddEditFieldViewModel model = new AddEditFieldViewModel();
            model.SelectGroupList = await _context.SelectGroups.ToListAsync();

            model.ICategories = await _context.Categories.Select(s => new SelectListItem
            {
                Text = s.Title,
                Value = s.Id.ToString()
            }).ToListAsync();
            model.SelectFiledGroupList = await _context.FieldGroups.ToListAsync();
            model.FildTypeModels = await _context.FieldTypes.Where(x => x.Id < 5).ToListAsync();

            if (id != 0)
            {
                using (_serviceProvider.GetRequiredService<ApplicationDbContext>())
                {

                    Field field = await _context.Fields.Where(a => a.Id == id).SingleOrDefaultAsync();
                    var select = _context.ProductCategoryFields.Where(a => a.IdFaild == id);
                    int[] b = new int[999];
                    int i = 0;
                    foreach (var item in select)
                    {

                        try
                        {
                            b[i] = item.IdCategory;
                        }
                        catch (Exception e)
                        {

                            throw;
                        }

                        i++;
                    }
                    if (field != null)
                    {
                        model.Id = field.Id;
                        model.Title = field.Title;
                        model.Type = field.Type;
                        model.IdSelectGroup = field.IdSelectGroup;
                        model.IdFieldGroup = field.IdFieldGroup;
                        model.IdCategory = b;
                    }

                }

            }


            return PartialView("AddEditField", model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditField(int Id, AddEditFieldViewModel model, string redirectUrl)
        {

            if (ModelState.IsValid)
            {
                var query = (from f in _context.Fields
                             join pcf in _context.ProductCategoryFields on f.Id equals pcf.IdFaild
                             select new ProductFieldViewModel()
                             {
                                 Type = f.Type,
                                 IdProduct = f.Id,
                                 IdCategory = pcf.IdCategory
                             }).Where(x => x.Type == 4);
                if (Id == 0)
                {

                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        //model.Type = (short)model.SelecteFieldTypeId;
                        int[] aa = model.IdCategory;
                        if (model.IdSelectGroup == 0)
                            model.IdSelectGroup = -1;
                        Field fieldModel = AutoMapper.Mapper.Map<AddEditFieldViewModel, Field>(model);

                        if (!CheckDuplicateDrp(model.IdCategory) || model.Type!=4)
                        {

                            db.Fields.Add(fieldModel);
                            await db.SaveChangesAsync();
                            var select = _context.Fields.LastOrDefault();
                            int ss = select.Id;

                            foreach (var item in aa)
                            {
                                ProductCategoryFields groupField = new ProductCategoryFields();
                                groupField.IdFaild = ss;
                                groupField.IdCategory = item;
                                db.ProductCategoryFields.Add(groupField);

                                //Add to ProductFields
                                ProductField pf = new ProductField();
                                var selectProducts = _context.Products.Where(x => x.IdCategory == item);
                                foreach (var itemProduct in selectProducts)
                                {
                                    pf.IdProduct = itemProduct.Id;
                                    pf.IdField = ss;
                                    pf.Value = null;
                                    db.ProductFields.Add(pf);
                                }
                                //Add to ProductFields
                            }

                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            TempData["Notif"] = Notification.ShowNotif("فیلد خودرد ها قبلا به این دسته بندی اضافه شده است", type: ToastType.red);

                            return PartialView("_Succefullyresponse", redirectUrl);
                        }
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);
                    return PartialView("_Succefullyresponse", redirectUrl);
                }
                //Edit
                else
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.ProductCategoryFields.RemoveRange(_context.ProductCategoryFields.Where(x => x.IdFaild == Id));

                        if (model.ICategories != null)
                        {
                        }
                        await db.SaveChangesAsync();
                        if (!CheckDuplicateDrp(model.IdCategory))
                        {
                            var selectType = _context.Fields.AsNoTracking().Where(x => x.Id == Id).AsNoTracking().FirstOrDefault();

                            if (selectType.Type == 4 && model.Type != 4)
                            {
                                int idProductField = _context.ProductFields.Where(x => x.IdField == selectType.Id).AsNoTracking().FirstOrDefault().Id;
                                db.ProductSelectedItems.RemoveRange(_context.ProductSelectedItems.Where(x => x.IdProductField == idProductField));
                                //await db.SaveChangesAsync();

                            }
                            model.Type = (short)model.Type;
                            if (model.IdSelectGroup == 0)
                                model.IdSelectGroup = -1;
                            int[] aa = model.IdCategory;
                            Field fieldModel = AutoMapper.Mapper.Map<AddEditFieldViewModel, Field>(model);
                            try
                            {
                                db.Fields.Update(fieldModel);

                            }
                            catch (Exception e)
                            {

                                throw;
                            }
                            foreach (var item in aa)
                            {
                                ProductCategoryFields groupField = new ProductCategoryFields();
                                groupField.IdFaild = Id;
                                groupField.IdCategory = item;
                                db.ProductCategoryFields.Add(groupField);
                            }
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            TempData["Notif"] = Notification.ShowNotif("فیلد خودرد ها قبلا به این دسته بندی اضافه شده است", type: ToastType.red);
                            return PartialView("_Succefullyresponse", redirectUrl);
                        }
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
            }

            if (Id == 0)
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.addError, type: ToastType.yellow);
            }
            else
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
            }

            model.SelectGroupList = await _context.SelectGroups.ToListAsync();
            //model.SelectGroupList = await _context.SelectGroups.Select(c => new SelectListItem()
            //{
            //    Text = c.Title,
            //    Value = c.Id.ToString()
            //}).ToListAsync();

            return PartialView("AddEditField", model);
        }

        public bool CheckDuplicateDrp(int[] idCat)
        {
            bool Exist = true;
            var query = (from f in _context.Fields
                         join pcf in _context.ProductCategoryFields on f.Id equals pcf.IdFaild
                         select new ProductFieldViewModel()
                         {
                             Type = f.Type,
                             IdProduct = f.Id,
                             IdCategory = pcf.IdCategory
                         }).Where(x => x.Type == 4);
            foreach (var item in idCat)
            {
                Exist = query.Where(x => x.IdCategory == item).Any();//آیتمی وجود دارد؟
                if (Exist == true)
                {
                    break;
                }
            }
            return Exist;
        }

        [HttpGet]
        public async Task<IActionResult> DeleteField(int Id)
        {
            var field = new Field();
            using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                field = await db.Fields.Where(a => a.Id == Id).SingleOrDefaultAsync();
                if (field == null)
                {
                    return RedirectToAction("Index");
                }
            }

            return PartialView("DeleteField", field.Title);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteField(int id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var field = await db.Fields.Where(a => a.Id == id).SingleOrDefaultAsync();

                    db.Fields.Remove(field);
                    await db.SaveChangesAsync();

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);
                    return PartialView("_Succefullyresponse", redirectUrl);
                }
            }

            TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.yellow);

            return RedirectToAction("Index");

        }
    }
}