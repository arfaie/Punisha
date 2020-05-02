using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.NewsTagses.Include(n => n.news).Include(n => n.tags).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.news = new SelectList(await _context.Newses.ToListAsync(), "Id", "Title");
            ViewBag.tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Title");

            var newstags = await _context.NewsTagses.SingleOrDefaultAsync(m => m.Id == id);
            if (newstags != null)
            {
                return PartialView("AddEdit", newstags);
            }

            return PartialView("AddEdit", new NewsTags());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, NewsTags model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    await _context.NewsTagses.AddAsync(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.NewsTagses.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            ViewBag.news = new SelectList(await _context.Newses.ToListAsync(), "Id", "Title");
            ViewBag.tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Title");

            return PartialView("AddEdit", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var newstags = await _context.NewsTagses.SingleOrDefaultAsync(b => b.Id == id);
            if (newstags == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", $"تگ{newstags.tags?.Title}برای خبر{newstags.news?.Title}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.NewsTagses.FirstOrDefaultAsync(c => c.Id == id);

                _context.NewsTagses.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

            return RedirectToAction("Index");
        }
    }
}