using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _Context;
        public readonly IServiceProvider _IServiceProvider;

        public OfferController(ApplicationDbContext context, IServiceProvider IServiceProvider)
        {
            _IServiceProvider = IServiceProvider;
            _Context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _Context.Offers.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddEditOffer(int Id)
        {
            var offer = new Offer();
            if (Id != 0)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    offer = await _Context.Offers.Where(b => b.Id == Id).SingleOrDefaultAsync();
                    if (offer == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("AddEditOffer", offer);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditOffer(int Id, Offer model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                if (Id == 0)
                {
                    using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Offers.Add(model);
                        await db.SaveChangesAsync();
                    }
                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);
                    return PartialView("_Succefullyresponse", redirecturl);
                }
                else
                {
                    using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Offers.Update(model);
                        await db.SaveChangesAsync();
                    }
                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);
                    return PartialView("_Succefullyresponse", redirecturl);
                }
            }
            else
            {
                if (Id == 0)
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);

                }
                else
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
                }
            }

            return PartialView("AddEditOffer", model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteOffer(int Id)
        {
            var offer = new Offer();
            if (Id != 0)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    offer = await _Context.Offers.Where(b => b.Id == Id).SingleOrDefaultAsync();
                    if (offer == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteOffer",offer.Title);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOffer(int Id, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await _Context.Offers.Where(b => b.Id == Id).SingleOrDefaultAsync();
                    db.Offers.Remove(model);
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