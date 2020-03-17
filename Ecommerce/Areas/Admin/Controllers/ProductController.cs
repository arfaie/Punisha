using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _appEnvironment;

        public ProductController(ApplicationDbContext context, IServiceProvider serviceProvider, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await (from p in _context.Products
                               join c in _context.Categories on p.IdCategory equals c.Id
                               join u in _context.Units on p.IdUnit equals u.Id
                               select new ProductViewModel()
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   IdUnit = p.IdUnit,
                                   UnitName = u.Title,
                                   IdCategory = p.IdCategory,
                                   CategoryName = c.Title,
                                   Code = p.Code,
                                   ImageName = p.ImageName,
                                   Inventory = p.Inventory,
                                   Issenoble = p.Issenoble,
                                   OrderPoint = p.OrderPoint,
                                   //Price = p.Price
                                   Price = (int)p.Price

                               }).ToListAsync();

            ViewBag.rootpath = "/upload/thumbnailimage/";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditProduct(int id)
        {
            AddEditProductViewModel model = new AddEditProductViewModel();
            model.CategoryList = await _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            model.UnitList = await _context.Units.Select(u => new SelectListItem
            {
                Text = u.Title,
                Value = u.Id.ToString()
            }).ToListAsync();

            if (id != 0)
            {
                using (_serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    Product product = await _context.Products.Where(p => p.Id == id).SingleOrDefaultAsync();
                    if (product != null)
                    {
                        model.Id = product.Id;
                        model.Name = product.Name;
                        model.OrderPoint = product.OrderPoint;
                        model.Price = product.Price;
                        model.Code = product.Code;
                        model.Inventory = product.Inventory;
                        model.Issenoble = product.Issenoble;
                        model.IdUnit = product.IdUnit;
                        model.IdCategory = product.IdCategory;
                        model.ImageName = product.ImageName;

                    }
                }
            }

            return PartialView("AddEditProduct", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditProduct(AddEditProductViewModel model, string ImgName, int id, IEnumerable<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                //upload audio
                var uploads = Path.Combine(_appEnvironment.WebRootPath, "upload\\normalimage\\");

                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);

                        using (var fs = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                            model.ImageName = filename;

                        }
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer();
                        img.Resize(uploads + filename,
                            _appEnvironment.WebRootPath + "\\upload\\thumbnailimage\\" + filename);
                    }
                }

                //upload audio

                if (id == 0)
                {
                    if (model.ImageName == null)
                    {
                        model.ImageName = "defaultpic.png";
                    }
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        Product productModel = AutoMapper.Mapper.Map<AddEditProductViewModel, Product>(model);
                        db.Products.Add(productModel);
                        await db.SaveChangesAsync();
                    }
                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);
                    //return PartialView("_Succefullyresponse", redirecturl);
                    //return Json(new { status = "success", message = "محصول با موفقیت ایجاد شد" });
                    return RedirectToAction("Index");
                }
                else
                {
                    if (model.ImageName == null)
                    {
                        model.ImageName = ImgName;
                    }
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        Product productModel = AutoMapper.Mapper.Map<AddEditProductViewModel, Product>(model);
                        db.Products.Update(productModel);
                        await db.SaveChangesAsync();
                    }
                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);
                    //return Json(new { status = "success", message = "اطلاعات محصول با موفقیت ویرایش شد" });
                    //return PartialView("_Succefullyresponse", redirecturl);
                    return RedirectToAction("Index");
                }
            }
            if (id == 0)
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.addError, type: ToastType.yellow);
            }
            else
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
            }

            model.CategoryList = await _context.Categories.Select(c => new SelectListItem()
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            model.UnitList = await _context.Units.Select(u => new SelectListItem()
            {
                Text = u.Title,
                Value = u.Id.ToString()
            }).ToListAsync();

            return PartialView("AddEditProduct", model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = new Product();
            using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                product = await db.Products.Where(p => p.Id == id).SingleOrDefaultAsync();
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
            }

            return PartialView("DeleteProduct", product.Name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int Id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var product = await _context.Products.Where(a => a.Id == Id).SingleOrDefaultAsync();

                    string sourcePath = Path.Combine(_appEnvironment.WebRootPath, "upload\\normalimage\\" + product.ImageName);
                    if (System.IO.File.Exists(sourcePath))
                    {
                        System.IO.File.Delete(sourcePath);
                    }

                    string sourcePath2 = Path.Combine(_appEnvironment.WebRootPath, "upload\\thumbnailimage\\" + product.ImageName);
                    if (System.IO.File.Exists(sourcePath2))
                    {
                        System.IO.File.Delete(sourcePath2);
                    }

                    db.Products.Remove(product);
                    await db.SaveChangesAsync();

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
            }
            TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.yellow);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetProductField(int id)
        {

            HttpContext.Session.SetInt32("IdProduct", id);

            var product = await _context.Products.Where(a => a.Id == id).SingleOrDefaultAsync();
            int idCategory = product.IdCategory;

            //دسته بندی کالای ما شامل کدام گروه فیلد هاست؟انتخاب همه گروه فیلدها 
            var SelectGroupField = _context.ProductCategoryFields.Where(a => a.IdCategory == idCategory);

            var SelectProductField = _context.ProductFields.Where(x => x.IdProduct == id);

            //کل فیلد ها در چند گروه اند؟انتخاب تمامی گروه ها بدون تکرار
            //List<int> lsFieldGroup = new List<int>();
            //foreach (var item in _context.Fields)
            //{
            //    if (!lsFieldGroup.Contains(item.IdFieldGroup))
            //    {
            //        lsFieldGroup.Add(item.IdFieldGroup);
            //    }
            //}


            //var model = (from f in _context.Fields
            //             join pg in SelectGroupField on f.Id equals pg.IdFaild
            //             join pf in SelectProductField on f.Id equals pf.IdField
            //             join psi in _context.ProductSelectedItems on pf.Id equals psi.IdProductField
            //             select new ProductFieldViewModel()
            //             {
            //                 IdItem = psi.IdItem,
            //                 IdCategory = pg.IdCategory,
            //                 IdField = f.Id,
            //                 IdFieldGroup = f.IdFieldGroup,
            //                 Type = f.Type,
            //                 Title = f.Title,
            //                 lsFieldGroup = lsFieldGroup,
            //                 IdProduct = pf.IdProduct,
            //                 IdProductField = pf.Id,
            //                 Value = pf.Value,
            //                 //CarId=_context.ProductSelectedItems.Where(x=>x.IdProductField==pf.Id),
            //                 fieldGroup = _context.FieldGroups.Where(x => x.Id == f.IdFieldGroup).FirstOrDefault().Title,
            //                 //CarListItems = new List<SelectListItem>(),

            //             }).ToList();

            //var IdProductField_ = model.Where(x => x.Type == 4).ToList();

            //var selectIdProductField = model.Where(x => x.Type == 4).FirstOrDefault();
            //if (selectIdProductField != null)
            //{
            //    int IdProductField = selectIdProductField.IdProductField;
            //    var select = _context.ProductSelectedItems.Where(a => a.IdProductField == IdProductField);
            //    int[] b = new int[999];
            //    int i = 0;
            //    foreach (var item in select)
            //    {

            //        try
            //        {
            //            b[i] = item.IdItem;
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

            var ss = (from f in _context.Fields
                      join fg in _context.FieldGroups on f.IdFieldGroup equals fg.Id
                      join pcf in _context.ProductCategoryFields on f.Id equals pcf.IdFaild
                      join pf in _context.ProductFields on f.Id equals pf.IdField
                      join p in _context.Products on pf.IdProduct equals p.Id
                      join psi in _context.ProductSelectedItems on pf.Id equals psi.IdProductField into dep
                      from dept in dep.DefaultIfEmpty()
                      join c in _context.Cars on dept.IdItem equals c.Id into dep1
                      from dept1 in dep1.DefaultIfEmpty()
                      select new FieldsViewModel()
                      {
                          IdField = f.Id,
                          IdFieldGroup = fg.Id,
                          idPrCatField = pcf.Id,
                          idProductField = pf.Id,
                          idProduct = p.Id,
                          IdCategory = pcf.IdCategory,
                          Type = f.Type,
                          FieldTitle = f.Title,
                          FieldGroTitle = fg.Title,
                          Value = pf.Value,
                          ProductName = p.Name,
                          CarName = dept1.Name == null ? "" : dept1.Name,
                          idCar = dept.IdItem == null ? -1 : dept.IdItem


                      }).ToList().Select(m => new FieldsViewModel
                      {
                          IdField = m.IdField,
                          IdFieldGroup = m.IdFieldGroup,
                          idPrCatField = m.idPrCatField,
                          idProductField = m.idProductField == null ? -1 : m.idProductField,
                          idProduct = m.idProduct,
                          IdCategory = m.IdCategory,
                          Type = m.Type,
                          FieldTitle = m.FieldTitle,
                          FieldGroTitle = m.FieldGroTitle,
                          Value = m.Value,
                          ProductName = m.ProductName,
                          CarName = m.CarName == null ? "" : m.CarName,
                          idCar = m.idCar == null ? -1 : m.idCar,
                          CarNames = CarNames(m.idProductField),
                          idCars = idCars(m.idProductField)

                      }).ToList();
            var selectProduct = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            ss = ss.Where(x => x.idProduct == id && x.IdCategory==selectProduct.IdCategory).ToList();

            var loopCnt = ss.GroupBy(x => x.IdFieldGroup);
            TempData["loopCnt"] = loopCnt;
            List<int> lsFieldGroup = new List<int>();
            int i = 0;
            foreach(var item in ss)
            {
                if (!lsFieldGroup.Contains(item.IdFieldGroup))
                {
                    lsFieldGroup.Add(item.IdFieldGroup);
                }
                i++;
            }
            TempData["loopCnt"] = lsFieldGroup;

            TempData["CarListItems"] = await _context.Cars.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();
            return PartialView("ProductField", ss);
        }
        public string CarNames(int id)
        {
            if (id != -1)
            {
                var select = _context.ProductSelectedItems.Where(x => x.IdProductField == id);
                List<string> cars = new List<string>();
                int i = 0;
                foreach (var item in select)
                {
                    string name = _context.Cars.Where(x => x.Id == item.IdItem).FirstOrDefault().Name;
                    cars.Add(name);

                }
                return String.Join("،", cars);
            }
            return null;
        }

        public int[] idCars(int IdProductField)
        {
            if (IdProductField != -1)
            {

                var select = _context.ProductSelectedItems.Where(x => x.IdProductField == IdProductField);
                int[] IDs = new int[select.Count()];
                int i = 0;
                foreach (var item in select)
                {
                    try
                    {
                        IDs[i] = item.IdItem;
                        i++;
                    }
                    catch (Exception e)
                    {

                        throw;
                    }

                }
                return IDs;
            }
            return null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEditProductField(FieldsViewModel model)
        {
            int[] lsIdField = (int[])TempData["lsIdField"];
            int IdProduct = (int)TempData["idProduct"];
            int IdProductField = (int)TempData["IdProductField"];
            int tp = model.Type;
            //check is Update
            var CarIds = Request.Form["bb"];
            var select = _context.ProductSelectedItems.Where(x => x.IdProductField == IdProductField);
            ProductSelectedItems selectedItems;

            if (model.idCars != null)
            {
                if (select.Any())
                {
                    _context.ProductSelectedItems.RemoveRange(select);
                    foreach (var item in model.idCars)
                    {
                        selectedItems = new ProductSelectedItems();
                        selectedItems.IdItem = Convert.ToInt16(item);
                        selectedItems.IdProductField = IdProductField;
                        _context.ProductSelectedItems.Add(selectedItems);
                    }
                }
                else
                {
                    foreach (var item in model.idCars)
                    {
                        selectedItems = new ProductSelectedItems();
                        selectedItems.IdItem = Convert.ToInt16(item);
                        selectedItems.IdProductField = IdProductField;
                        _context.ProductSelectedItems.Add(selectedItems);
                    }
                }
            }
            

            foreach (var item in lsIdField)
            {
                var selectPF = _context.ProductFields.Where(x => x.IdField == item && x.IdProduct == IdProduct).FirstOrDefault();
                string value = Request.Form[item.ToString()];
                selectPF.Value = value;
                _context.ProductFields.Update(selectPF);
            }
            var db = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            db.SaveChanges();

            TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

            return RedirectToAction("Index");
        }
    }
}