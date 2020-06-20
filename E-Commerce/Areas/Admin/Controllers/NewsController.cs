using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
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

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        public NewsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            ViewBag.rootpath = "/upload/thumbnailimage/";

            return View(await _context.Newses.Include(n => n.NewCategories).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.newscategory = new SelectList(await _context.NewsCategories.ToListAsync(), "Id", "Title");

            var news = await _context.Newses.FirstOrDefaultAsync(c => c.Id == id);
            if (news != null)
            {
                return PartialView("AddEdit", news);
            }

            return PartialView("AddEdit", new News());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(News model, string imgName, string id, IEnumerable<IFormFile> files, string strDate)
        {
            if (ModelState.IsValid)
            {
                string pDate = strDate.PersianToEnglish();
                model.Date = pDate.ToGeorgianDateTime();

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


                if (String.IsNullOrWhiteSpace(id))
                {
                    if (model.ImageName == null)
                    {
                        model.ImageName = "defaultpic.png";
                    }

                    await _context.Newses.AddAsync(model);

                    await _context.SaveChangesAsync();


                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

                    return RedirectToAction("Index");
                }

                if (model.ImageName == null)
                {
                    model.ImageName = imgName;
                }

             

                _context.Newses.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

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

            ViewBag.newscategory = new SelectList(await _context.NewsCategories.ToListAsync(), "Id", "Title");

            return PartialView("AddEdit", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var news = await _context.Newses.FirstOrDefaultAsync(c => c.Id == id);
            if (news == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", news.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var news = await _context.Newses.SingleOrDefaultAsync(a => a.Id == id);

                var sourcePath = Path.Combine(_env.WebRootPath, "upload\\normalimage\\" + news.ImageName);
                if (System.IO.File.Exists(sourcePath))
                {
                    System.IO.File.Delete(sourcePath);
                }

                var sourcePath2 = Path.Combine(_env.WebRootPath, "upload\\thumbnailimage\\" + news.ImageName);
                if (System.IO.File.Exists(sourcePath2))
                {
                    System.IO.File.Delete(sourcePath2);
                }

                var newstags = await _context.NewsTagses.Where(x => x.IdNews == id).ToListAsync();
                _context.NewsTagses.RemoveRange(newstags);

                _context.Newses.Remove(news);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }
            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> ck()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ck(string text)
        //{
        //    string a = text;
        //    return View();
        //}

        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                _env.WebRootPath, "/upload/News",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/upload/News/"}{fileName}";


            return Json(new { uploaded = true, url });
        }


    }
}