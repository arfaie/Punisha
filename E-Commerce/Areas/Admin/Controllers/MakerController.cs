﻿using System;
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
    public class MakerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MakerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Makers.ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEditMaker(string id)
        {
            var maker = await _context.Makers.SingleOrDefaultAsync(m => m.Id == id);
            if (maker != null)
            {
                return PartialView("AddEditMaker", maker);
            }

            return PartialView("AddEditMaker", new Maker());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditMaker(string id, Maker maker, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(id))
                {
                    _context.Makers.Add(maker);
                    await _context.SaveChangesAsync();
                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.Makers.Update(maker);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            ViewBag.States = new SelectList(await _context.States.ToListAsync(), "Id", "Title");

            return PartialView("AddEditMaker", maker);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteMaker(string id)
        {
            var maker = await _context.Makers.SingleOrDefaultAsync(m => m.Id == id);
            if (maker == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("DeleteMaker", maker.Name);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMaker(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.Makers.FirstOrDefaultAsync(c => c.Id == id);

                _context.Makers.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

            return RedirectToAction("Index");
        }
    }
}