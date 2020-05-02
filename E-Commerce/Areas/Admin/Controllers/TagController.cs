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
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tags.Include(t => t.NewsTagses).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            var tag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);
            if (tag != null)
            {
                return PartialView("AddEdit", tag);
            }

            return PartialView("AddEdit", new Tag());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, Tag model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    await _context.Tags.AddAsync(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.Tags.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            return PartialView("AddEdit", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var tag = await _context.Tags.SingleOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", tag.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);

                _context.Tags.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

            return RedirectToAction("Index");
        }
    }
}