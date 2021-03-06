﻿using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ECommerce.Helpers;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfferController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            var select = await _context.Offers.Include(o => o.OfferItems).ToListAsync();
            return View(select);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.UserGroups = new SelectList(await _context.UserGroups.ToListAsync(), "Id", "Title");

            var offer = await _context.Offers.SingleOrDefaultAsync(b => b.Id == id);
            if (offer != null)
            {
                return PartialView("AddEdit", offer);
            }

            return PartialView("AddEdit", new Offer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, Offer model, string redirectUrl, string strEndDate, string strStartDate)
        {
            if (ModelState.IsValid)
            {
                string pEndDate = strEndDate.PersianToEnglish();
                model.EndDate = pEndDate.ToGeorgianDateTime();

                string pStartDate = strStartDate.PersianToEnglish();
                model.StartDate = pStartDate.ToGeorgianDateTime();

                if (String.IsNullOrWhiteSpace(id))
                {
                    await _context.Offers.AddAsync(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.Offers.Update(model);
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
            var offer = await _context.Offers.SingleOrDefaultAsync(b => b.Id == id);
            if (offer == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", offer.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.Offers.SingleOrDefaultAsync(b => b.Id == id);

                _context.Offers.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Red);
            return RedirectToAction("Index");
        }
    }
}