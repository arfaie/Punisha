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
    public class BlogCommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogComments.Include(b => b.ApplicationUser).Include(b => b.News).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.user = new SelectList(await _context.Users.ToListAsync(), "Id", "FullName");
            ViewBag.blog = new SelectList(await _context.Newses.ToListAsync(), "Id", "Title");

            var blogComment = await _context.BlogComments.FirstOrDefaultAsync(c => c.Id == id);
            if (blogComment != null)
            {
                return PartialView("AddEdit", blogComment);
            }

            return PartialView("AddEdit", new BlogComment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, BlogComment model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            ViewBag.user = new SelectList(await _context.Users.ToListAsync(), "Id", "FullName");
            ViewBag.blog = new SelectList(await _context.Newses.ToListAsync(), "Id", "Title");

            return PartialView("AddEdit", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var blogcomment = await _context.BlogComments.FirstOrDefaultAsync(c => c.Id == id);
            if (blogcomment == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", $"این کامنت");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.BlogComments.FirstOrDefaultAsync(c => c.Id == id);

                _context.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

            return RedirectToAction("Index");
        }
    }
}