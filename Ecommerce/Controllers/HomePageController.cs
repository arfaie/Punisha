using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Controllers
{
   
    public class HomePageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iserviceProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomePageController(ApplicationDbContext context, IServiceProvider iserviceProvider, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _iserviceProvider = iserviceProvider;
            _userManager = userManager;
        }

        public async Task<IActionResult> Home()
        {
            MultiModelsHome modelsHome = new MultiModelsHome();
            var model = await _context.Sliders.ToListAsync();

            var modelProduct = await (from p in _context.Products
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
                                          Price = (int)p.Price,
                                          OldPrice = (int)p.OldPrice

                                      }).ToListAsync();

            modelsHome.Sliders = model;
            modelsHome.ProductViewModels = modelProduct;

            var select = _context.Cars.ToList();

            ViewBag.Cars = select;

            var select2 = _context.Categories.ToList();

            ViewBag.Categories = select2;

            ViewBag.rootpath = "/upload/normalimage/";
            return View(modelsHome);
        }

        public IEnumerable<ProductFieldViewModel> GetProductFilds(int id)
        {

            HttpContext.Session.SetInt32("IdProduct", id);
            List<int> lsFieldGroup = new List<int>();
            var product = _context.Products.Where(a => a.Id == id).SingleOrDefault();
            int idCategory = product.IdCategory;

            //دسته بندی کالای ما شامل کدام گروه فیلد هاست؟انتخاب همه گروه فیلدها 
            var SelectGroupField = _context.ProductCategoryFields.Where(a => a.IdCategory == idCategory);

            var SelectProductField = _context.ProductFields.Where(x => x.IdProduct == id);

            //کل فیلد ها در چند گروه اند؟انتخاب تمامی گروه ها بدون تکرار
            foreach (var item in _context.Fields)
            {
                if (!lsFieldGroup.Contains(item.IdFieldGroup))
                {
                    lsFieldGroup.Add(item.IdFieldGroup);
                }
            }

            var model = (from f in _context.Fields
                         join pg in SelectGroupField on f.Id equals pg.IdFaild
                         join pf in SelectProductField on f.Id equals pf.IdField
                         //join psi in _context.ProductSelectedItems on pf.Id equals psi.IdProductField
                         select new ProductFieldViewModel()
                         {
                             // CarNames = ""
                             IdCategory = pg.IdCategory,
                             IdField = f.Id,
                             IdFieldGroup = f.IdFieldGroup,
                             Type = f.Type,
                             Title = f.Title,
                             lsFieldGroup = lsFieldGroup,
                             IdProduct = pf.IdProduct,
                             IdProductField = pf.Id,
                             Value = pf.Value,
                             //CarId=_context.ProductSelectedItems.Where(x=>x.IdProductFie==pf.Id),
                             fieldGroup = _context.FieldGroups.Where(x => x.Id == f.IdFieldGroup).FirstOrDefault().Title,
                             //CarListItems = new List<SelectListItem>(),

                         }).ToList().Select(n => new ProductFieldViewModel
                         {
                             CarNames = CarNames(n.IdProductField),
                             IdCategory = n.IdCategory,
                             IdField = n.Id,
                             IdFieldGroup = n.IdFieldGroup,
                             Type = n.Type,
                             Title = n.Title,
                             lsFieldGroup = lsFieldGroup,
                             IdProduct = n.IdProduct,
                             IdProductField = n.Id,
                             Value = n.Value,
                             fieldGroup = _context.FieldGroups.Where(x => x.Id == n.IdFieldGroup).FirstOrDefault().Title,
                             CarListItems = n.CarListItems,
                         });

            //var IdProductField_ = model.Where(x => x.Type == 4).ToList();

            var selectIdProductField = model.Where(x => x.Type == 4).FirstOrDefault();
            if (selectIdProductField != null)
            {
                int IdProductField = selectIdProductField.IdProductField;
                var select = _context.ProductSelectedItems.Where(a => a.IdProductField == IdProductField);
                int[] b = new int[999];
                int i = 0;
                foreach (var item in select)
                {

                    try
                    {
                        b[i] = item.IdItem;
                    }
                    catch (Exception e)
                    {

                        throw;
                    }

                    i++;
                }
                TempData["key"] = b;
            }

            try
            {
                model.FirstOrDefault().CarListItems = _context.Cars.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
            }
            catch (Exception e)
            {

            }

            return model;
        }

        public string CarNames(int id)
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

        public async Task<IActionResult> ProductDetailes(int id)
        {
            MultiModelsProductDetailes multiModelsProductDetailes = new MultiModelsProductDetailes();

            var modelProduct = await (from p in _context.Products
                                      join c in _context.Categories on p.IdCategory equals c.Id
                                      join u in _context.Units on p.IdUnit equals u.Id
                                      where p.Id == id
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
                                          Price = (int)p.Price,
                                          OldPrice = (int)p.OldPrice

                                      }).ToListAsync();



            multiModelsProductDetailes.ProductViewModels = modelProduct;
            multiModelsProductDetailes.ProductFieldViewModels = GetProductFilds(id);

            var select = _context.Cars.ToList();

            ViewBag.Cars = select;

            var select2 = _context.Categories.ToList();

            ViewBag.Categories = select2;

            return View(multiModelsProductDetailes);


        }

        public IActionResult Borrow(int productid)
        {
            using (var db = _iserviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                //کنترل اینکه آیدی ارسال شده وجود دارد یا خیر
                var query = db.Products.Where(b => b.Id == productid).SingleOrDefault();
                if (query == null)
                {
                    //return RedirectToAction("BookDetails", bookid);
                    return Json(new { status = "fail", message = "این محصول در انبار وجود ندارد" });
                }

                if (query.Inventory == 0)
                {
                    //اگر موجودی نبود
                    //return RedirectToAction("BookDetails", bookid);
                    return Json(new { status = "success", message = "این محصول موجود نیست" });
                }
                else
                {
                    //اگر کتاب موجودی داشت
                    if (Request.Cookies["S#$51%^Lm*A!2@m"] == null)
                    {
                        Response.Cookies.Append("S#$51%^Lm*A!2@m", "," + productid + ",",
                            new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) });

                        return Json(new { status = "success", message = "محصول به لیست درخواستی شما اضافه شد", sabadcount = 1 });
                    }
                    else
                    {
                        string cookicontent = Request.Cookies["S#$51%^Lm*A!2@m"].ToString();
                        //کتاب قبلا به لیست درخواستی اضافه نشده باشد
                        if (cookicontent.Contains("," + productid + ","))
                        {
                            //new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) });
                            return Json(new { status = "success", message = "این محصول از قبل در لیست درخواستی شما وجود دارد" }); ;
                        }
                        else
                        {
                            cookicontent += "," + productid + ",";
                            Response.Cookies.Append("S#$51%^Lm*A!2@m", cookicontent,
                                new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) });


                            string[] requestbookcount = cookicontent.Split(',');
                            requestbookcount = requestbookcount.Where(r => r != "").ToArray();
                            TempData["Notif"] = Notification.ShowNotif(MessageType.Add, ToastType.green);
                            return Json(new
                            {
                                status = "success",
                                message = "محصول به لیست درخواستی شما اضافه شد",
                                sabadcount = requestbookcount.Count()
                            });
                        }
                    }
                }
            }

            //return null;
        }

       

       

       

        public async Task<IActionResult> SearchProduct(int carsearch, int categorysearch)
        {
            try
            {
                List<ProductViewModel> productViewModels = new List<ProductViewModel>();

                //if (categorysearch != 0 && carsearch != 0)
                //{
                IEnumerable<ProductFieldViewModel> model = null;


                var SelectGroupField = _context.ProductCategoryFields.ToList();

                var SelectProductField = _context.ProductFields.ToList();

                if (categorysearch != 0 && carsearch != 0)
                {

                    model = (from f in _context.Fields
                             join pg in SelectGroupField on f.Id equals pg.IdFaild
                             join pf in SelectProductField on f.Id equals pf.IdField
                             join pro in _context.ProductSelectedItems on pf.Id equals pro.IdProductField

                             //join psi in _context.ProductSelectedItems on pf.Id equals psi.IdProductField
                             select new ProductFieldViewModel()
                             {
                                 // CarNames = ""
                                 Item = pro.IdItem,
                                 IdCategory = pg.IdCategory,
                                 IdField = f.Id,
                                 IdFieldGroup = f.IdFieldGroup,
                                 Type = f.Type,
                                 Title = f.Title,
                                 IdProduct = pf.IdProduct,
                                 IdProductField = pf.Id,
                                 Value = pf.Value,
                                 //CarId=_context.ProductSelectedItems.Where(x=>x.IdProductFie==pf.Id),
                                 fieldGroup = _context.FieldGroups.Where(x => x.Id == f.IdFieldGroup).FirstOrDefault().Title,
                                 //CarListItems = new List<SelectListItem>(),

                             }).Where(x => x.IdCategory == categorysearch && x.Item == carsearch);
                }
                else if (categorysearch != 0 && carsearch == 0)
                {
                    model = (from f in _context.Fields
                             join pg in SelectGroupField on f.Id equals pg.IdFaild
                             join pf in SelectProductField on f.Id equals pf.IdField
                             join pro in _context.ProductSelectedItems on pf.Id equals pro.IdProductField

                             //join psi in _context.ProductSelectedItems on pf.Id equals psi.IdProductField
                             select new ProductFieldViewModel()
                             {
                                 // CarNames = ""
                                 Item = pro.IdItem,
                                 IdCategory = pg.IdCategory,
                                 IdField = f.Id,
                                 IdFieldGroup = f.IdFieldGroup,
                                 Type = f.Type,
                                 Title = f.Title,
                                 IdProduct = pf.IdProduct,
                                 IdProductField = pf.Id,
                                 Value = pf.Value,
                                 //CarId=_context.ProductSelectedItems.Where(x=>x.IdProductFie==pf.Id),
                                 fieldGroup = _context.FieldGroups.Where(x => x.Id == f.IdFieldGroup).FirstOrDefault().Title,
                                 //CarListItems = new List<SelectListItem>(),

                             }).Where(x => x.IdCategory == categorysearch);
                }
                else if (categorysearch == 0 && carsearch != 0)
                {
                    model = (from f in _context.Fields
                             join pg in SelectGroupField on f.Id equals pg.IdFaild
                             join pf in SelectProductField on f.Id equals pf.IdField
                             join pro in _context.ProductSelectedItems on pf.Id equals pro.IdProductField

                             //join psi in _context.ProductSelectedItems on pf.Id equals psi.IdProductField
                             select new ProductFieldViewModel()
                             {
                                 // CarNames = ""
                                 Item = pro.IdItem,
                                 IdCategory = pg.IdCategory,
                                 IdField = f.Id,
                                 IdFieldGroup = f.IdFieldGroup,
                                 Type = f.Type,
                                 Title = f.Title,
                                 IdProduct = pf.IdProduct,
                                 IdProductField = pf.Id,
                                 Value = pf.Value,
                                 //CarId=_context.ProductSelectedItems.Where(x=>x.IdProductFie==pf.Id),
                                 fieldGroup = _context.FieldGroups.Where(x => x.Id == f.IdFieldGroup).FirstOrDefault().Title,
                                 //CarListItems = new List<SelectListItem>(),

                             }).Where(x => x.Item == carsearch);
                }
                //.Where(x => x.IdCategory == categorysearch && x.Item == carsearch);
                //if (categorysearch != 0 && carsearch != 0)
                //{
                //    model.Where(x => x.IdCategory == categorysearch && x.Item == carsearch);
                //}
                //else if (carsearch != 0 && categorysearch == 0)
                //{
                //    model.Where(x => x.Item == carsearch);
                //}
                //else
                //{
                //    model.Where(x => x.IdCategory == categorysearch);
                //}

                foreach (var item in model)
                {
                    ProductViewModel obModel = new ProductViewModel();
                    var select = _context.Products.Where(x => x.Id == item.IdProduct).FirstOrDefault();
                    obModel.Name = select.Name;
                    obModel.ImageName = select.ImageName;
                    obModel.OldPrice = (int)select.OldPrice;
                    obModel.Price = (int)select.Price;

                    productViewModels.Add(obModel);
                }
                // }

                var select3 = _context.Cars.ToList();

                ViewBag.Cars = select3;

                var select2 = _context.Categories.ToList();

                ViewBag.Categories = select2;

                MultiModelSearchProduct multiModelSearchProduct = new MultiModelSearchProduct();

                multiModelSearchProduct.ProductViewModels = productViewModels;

                var selectcategories = _context.Categories.ToList();

                multiModelSearchProduct.Categories = selectcategories;

                var selectcars = _context.Cars.ToList();

                multiModelSearchProduct.Cars = selectcars;

                ViewBag.imagepath = "/upload/normalimage/";

                var selec2t = _context.Cars.ToList();

                ViewBag.Cars = selec2t;

                var select22 = _context.Categories.ToList();

                ViewBag.Categories = select22;

                return View("SearchProduct", multiModelSearchProduct);
            }
            catch (Exception e)
            {
                var selec2t = _context.Cars.ToList();

                ViewBag.Cars = selec2t;

                var select22 = _context.Categories.ToList();

                ViewBag.Categories = select22;

                return RedirectToAction("Home");
            }

            

        }

       

       


    }
}