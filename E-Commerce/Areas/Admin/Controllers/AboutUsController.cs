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
    public class AboutUsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AboutUsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.AboutUses.FirstOrDefaultAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(string id)
        {
            var aboutuse = await _context.AboutUses.FirstOrDefaultAsync(c => c.Id == id);
            if (aboutuse != null)
            {
                return PartialView("Edit", aboutuse);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AboutUs model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    TempData["Notification"] = Notification.ShowNotif("Not Found", ToastType.Red);
                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.AboutUses.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            return PartialView("Edit", model);
        }
    }
}