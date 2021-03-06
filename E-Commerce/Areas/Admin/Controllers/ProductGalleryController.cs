﻿using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
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
using Microsoft.Extensions.DependencyInjection;
using SessionExtensions = ECommerce.Helpers.SessionExtensions;

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

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index", "Product");
            }

            ViewBag.rootpath = "/upload/thumbnailimage/";

            HttpContext.Session.SetString("IDProduct", id);

            var select = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            ViewBag.productname = select.Name;

            return View(await _context.ProductGalleries.Where(g => g.ProductId == id).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");

            var productGallery = await _context.ProductGalleries.FirstOrDefaultAsync(c => c.Id == id);
            if (productGallery != null)
            {
                return PartialView("AddEdit", productGallery);
            }

            return PartialView("AddEdit", new ProductGallery());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddEdit(ProductGallery model, string id, IEnumerable<IFormFile> files, string imgName)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var stList = new List<string>();

        //        var upload = Path.Combine(_env.WebRootPath.Replace("\\", "/") + Helper.NormalImagePath);
        //        foreach (var file in files)
        //        {
        //            if (file != null && file.Length > 0)
        //            {
        //                var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);

        //                await using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
        //                {
        //                    await file.CopyToAsync(fs);
        //                    stList.Add(filename);
        //                    //model.Image = filename;
        //                }

        //                var img = new ImageResizer();
        //                img.Resize(upload + filename, _env.WebRootPath + Helper.ThumbnailImagePath + filename);
        //            }
        //        }
        //        if (model.Image == null)
        //        {
        //            model.Image = "defaultpic.png";
        //        }
        //        if (!String.IsNullOrWhiteSpace(id))
        //        {
        //            foreach (var variable in stList)
        //            {
        //                model.Image = variable;
        //                model.ProductId = HttpContext.Session.GetString("IDProduct");
        //                //model.ProductId = (int)HttpContext.Session.GetInt32("ImageProductKey");
        //                _context.ProductGalleries.Add(model);
        //                _context.SaveChanges();
        //            }

        //            return Json(new { Status = "success", Message = "عکس با موفقیت ثبت شد" });
        //        }

        //        foreach (var variable in stList)
        //        {
        //            model.Image = variable;

        //            _context.ProductGalleries.Update(model);
        //            _context.SaveChanges();
        //        }

        //        return Json(new { Status = "success", Message = "عکس با موفقیت ویرایش شد" });
        //    }

        //    ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");

        //    var list = new List<string>();
        //    foreach (var validation in ViewData.ModelState.Values)
        //    {
        //        list.AddRange(validation.Errors.Select(error => error.ErrorMessage));
        //    }

        //    return Json(new { Status = "error", error = list });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(ProductGallery model, string imgName, string id, IEnumerable<IFormFile> files, string redirectUrl)
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
                            model.Image = filename;
                        }
                        var img = new ImageResizer();
                        img.Resize(uploads + filename, _env.WebRootPath + "\\upload\\thumbnailimage\\" + filename);
                    }
                }
                //upload image

                if (String.IsNullOrWhiteSpace(id))
                {
                    if (model.Image == null)
                    {
                        model.Image = "defaultpic.png";
                    }

                    model.ProductId = HttpContext.Session.GetString("IDProduct");
                    _context.ProductGalleries.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
                    string path = $"{redirectUrl}/index/{HttpContext.Session.GetString("IDProduct")}";
                    return PartialView("_SuccessfulResponse", path);
                }

                if (model.Image == null)
                {
                    model.Image = imgName;
                }

                model.ProductId = HttpContext.Session.GetString("IDProduct");
                _context.ProductGalleries.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
                string path1 = $"{redirectUrl}/index/{HttpContext.Session.GetString("IDProduct")}";
                return PartialView("_SuccessfulResponse", path1);
            }
            if (!String.IsNullOrWhiteSpace(id))
            {
                TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, ToastType.Yellow);
            }
            else
            {
                TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, ToastType.Yellow);
            }

            ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");

            return PartialView("AddEdit", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var productGallery = await _context.ProductGalleries.FirstOrDefaultAsync(c => c.Id == id);
            if (productGallery == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", productGallery.Image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.ProductGalleries.SingleOrDefaultAsync(b => b.Id == id);

                var sourcePath = Path.Combine(_env.WebRootPath.Replace("\\", "/") + Helper.NormalImagePath, model.Image);
                if (System.IO.File.Exists(sourcePath))
                {
                    System.IO.File.Delete(sourcePath);
                }

                var sourcePath2 = Path.Combine(_env.WebRootPath.Replace("\\", "/") + Helper.ThumbnailImagePath, model.Image);
                if (System.IO.File.Exists(sourcePath2))
                {
                    System.IO.File.Delete(sourcePath2);
                }

                _context.ProductGalleries.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Red);
            return RedirectToAction("Index");
        }
    }
}