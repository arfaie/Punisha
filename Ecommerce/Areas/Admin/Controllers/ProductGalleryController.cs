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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductGalleryController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly IServiceProvider _ServiceProvider;
        private readonly IHostingEnvironment _appEnvironmen;

        public ProductGalleryController(ApplicationDbContext context, IServiceProvider serviceProvider, IHostingEnvironment appEnvironmen)
        {
            _Context = context;
            _ServiceProvider = serviceProvider;
            _appEnvironmen = appEnvironmen;
        }

        public IActionResult Index(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Product");
            }

            var model = _Context.ProductGalleries.Where(g => g.IdProduct == id).ToList();

            ViewBag.rootpath = "/upload/thumbnailimage/";
            HttpContext.Session.SetInt32("ImageProductKey", id);

            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditProductGallery(int id)
        {
            AddEditProductGallery model = new AddEditProductGallery();

            model.ProductListItems = _Context.Products.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();
            if (id != 0)
            {
                using (var db = _ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    ProductGallery image = _Context.ProductGalleries.Where(n => n.Id == id).SingleOrDefault();

                    if (image != null)
                    {
                        model.Id = image.Id;
                        model.IdProduct = image.IdProduct;
                        model.Image = image.Image;
                    }
                }
            }
            return PartialView("AddEditProductGallery", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditProductGallery(AddEditProductGallery model, int id, IEnumerable<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                List<string> stList = new List<string>();

                var upload = Path.Combine(_appEnvironmen.WebRootPath, "upload\\normalimage\\");
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

                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer();
                        img.Resize(upload + filename, _appEnvironmen.WebRootPath + "\\upload\\thumbnailimage\\" + filename);
                    }
                }
                if (model.Image == null)
                {
                    model.Image = "defaultpic.png";
                }
                if (id == 0)
                {
                    using (var db = _ServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        foreach (var VARIABLE in stList)
                        {
                            model.Image = VARIABLE;
                            model.IdProduct = (int)HttpContext.Session.GetInt32("ImageProductKey");
                            ProductGallery imagemodel = AutoMapper.Mapper.Map<AddEditProductGallery, ProductGallery>(model);
                            db.ProductGalleries.Add(imagemodel);
                            db.SaveChanges();
                        }

                    }
                    return Json(new { Status = "success", Message = "عکس با موفقیت ثبت شد" });
                }
                else
                {
                    using (var db = _ServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        foreach (var VARIABLE in stList)
                        {
                            model.Image = VARIABLE;
                            model.IdProduct = (int)HttpContext.Session.GetInt32("ImageProductKey");
                            ProductGallery imagemodel = AutoMapper.Mapper.Map<AddEditProductGallery, ProductGallery>(model);
                            db.ProductGalleries.Update(imagemodel);
                            db.SaveChanges();
                        }


                    }
                    return Json(new { Status = "success", Message = "عکس با موفقیت ویرایش شد" });
                }

            }
            model.ProductListItems = _Context.Products.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();

            var list = new List<string>();
            foreach (var validation in ViewData.ModelState.Values)
            {
                list.AddRange(validation.Errors.Select(error => error.ErrorMessage));
            }

            return Json(new { Status = "error", error = list });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProductGallery(int Id)
        {
            var ProductGallery = new ProductGallery();
            if (Id != 0)
            {
                using (var db = _ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    ProductGallery = await _Context.ProductGalleries.Where(b => b.Id == Id).SingleOrDefaultAsync();
                    if (ProductGallery == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteProductGallery");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductGallery(int Id, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await _Context.ProductGalleries.Where(b => b.Id == Id).SingleOrDefaultAsync();

                    string sourcePath = Path.Combine(_appEnvironmen.WebRootPath, "upload\\normalimage\\" + model.Image);
                    if (System.IO.File.Exists(sourcePath))
                    {
                        System.IO.File.Delete(sourcePath);
                    }

                    string sourcePath2 = Path.Combine(_appEnvironmen.WebRootPath, "upload\\thumbnailimage\\" + model.Image);
                    if (System.IO.File.Exists(sourcePath2))
                    {
                        System.IO.File.Delete(sourcePath2);
                    }

                    db.ProductGalleries.Remove(model);
                    await db.SaveChangesAsync();
                }
                TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);
                return PartialView("_Succefullyresponse", redirecturl);
            }
            else
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.red);
                return RedirectToAction("Index");
            }
        }
    }
}