using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _IServiceProvider;
        private readonly IHostingEnvironment _appEnvironmen;

        public SliderController(ApplicationDbContext context, IServiceProvider serviceProvider, IHostingEnvironment appEnvironmen)
        {
            _context = context;
            _IServiceProvider = serviceProvider;
            _appEnvironmen = appEnvironmen;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _context.Sliders.ToListAsync();
            ViewBag.rootpath = "/upload/thumbnailimage/";
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddEditSlider(int Id)
        {
            Slider model = new Slider();
            if (Id != 0)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    Slider slid = _context.Sliders.Where(n => n.Id == Id).SingleOrDefault();
                    if (slid != null)
                    {
                        model.Id = slid.Id;
                        model.Image = slid.Image;
                    }

                }
            }

            return PartialView("AddEditSlider", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEditSlider(int id, Slider model, IEnumerable<IFormFile> files, string Imgename)
        {
            if (ModelState.IsValid)
            {
                var uploud = Path.Combine(_appEnvironmen.WebRootPath, "upload\\normalimage\\");
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fs = new FileStream(Path.Combine(uploud, filename), FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                            model.Image = filename;
                        }
                        InsertShowImage.ImageResizer image = new InsertShowImage.ImageResizer();
                        image.Resize(uploud + filename, _appEnvironmen.WebRootPath + "\\upload\\thumbnailimage\\" + filename);

                    }
                }

                if (id == 0)
                {
                    if (model.Image == null)
                    {
                        model.Image = "defaultpic.png";
                    }
                    using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Sliders.Add(model);
                        await db.SaveChangesAsync();
                    }
                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);
                    return Json(new { Status = "success" });
                }
                else
                {
                    if (model.Image == null)
                    {
                        model.Image = Imgename;
                    }
                    using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Sliders.Update(model);
                        await db.SaveChangesAsync();
                    }
                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);
                    return Json(new { Status = "success" });
                }

            }
            var list = new List<string>();
            foreach (var validation in ViewData.ModelState.Values)
            {
                list.AddRange(validation.Errors.Select(error => error.ErrorMessage));
            }
            return Json(new { Status = "error" });
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSlider(int Id)
        {
            var Slider = new Slider();
            if (Id != 0)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    Slider = await _context.Sliders.Where(b => b.Id == Id).SingleOrDefaultAsync();
                    if (Slider == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteSlider");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSlider(int Id, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await _context.Sliders.Where(b => b.Id == Id).SingleOrDefaultAsync();

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

                    db.Sliders.Remove(model);
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

