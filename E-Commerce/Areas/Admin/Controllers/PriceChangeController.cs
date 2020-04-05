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
    //[Authorize(Roles = "Admin")]
    public class PriceChangeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PriceChangeController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.PriceChanges.Include(p => p.Product).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");

            var select = await _context.PriceChanges.FirstOrDefaultAsync(p => p.Id == id);

            if (select != null)
            {
                return PartialView("AddEdit", select);
            }

            return PartialView("AddEdit", new PriceChange());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(PriceChange model)
        {
            if (ModelState.IsValid)
            {
                //if (String.IsNullOrEmpty(id))
                //{
                    _context.PriceChanges.Add(model);
                    await _context.SaveChangesAsync();

                    //TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

                    //return PartialView("_SuccessfulResponse", redirecturl);

                //}

                //_context.PriceChanges.Update(model);
                //await _context.SaveChangesAsync();

                //TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                //return PartialView("_SuccessfulResponse", redirecturl);
            }

            return PartialView("AddEdit", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var select = await _context.PriceChanges.FirstOrDefaultAsync(p => p.Id == id);
            if (select == null)
            {
                return null;
            }

            return PartialView("Delete", $"{select.Product?.Name} {select.Date}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.PriceChanges.FirstOrDefaultAsync(p => p.Id == id);

                _context.PriceChanges.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

            return RedirectToAction("Index");

        }
    }
}