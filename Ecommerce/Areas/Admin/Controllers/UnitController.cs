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

namespace Ecommerce.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UnitController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iServiceProvider;

        public UnitController(ApplicationDbContext context, IServiceProvider iServiceProvider)
        {
            _context = context;
            _iServiceProvider = iServiceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Units.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditUnit(int id)
        {
            var unit = new Unit(); ;
            if (id != 0)
            {
                using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    unit = await _context.Units.Where(c => c.Id == id).SingleOrDefaultAsync();
                    if (unit == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("AddEditUnit", unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditUnit(int id, Unit model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Units.Add(model);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
                else
                {
                    using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Units.Update(model);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
            }
            else
            {

                if (id == 0)
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.addError, type: ToastType.yellow);
                }
                else
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
                }

                return PartialView("AddEditUnit", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var unit = new Unit();
            if (id != 0)
            {
                using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    unit = await _context.Units.Where(c => c.Id == id).SingleOrDefaultAsync();
                    if (unit == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteUnit", unit.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUnit(int id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await _context.Units.Where(c => c.Id == id).SingleOrDefaultAsync();
                    db.Units.Remove(model);
                    db.SaveChanges();
                }

                TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, ToastType.red);
                return PartialView("_Succefullyresponse", redirectUrl);
            }
            else
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.yellow);

                return RedirectToAction("Index");
            }
        }

    }
}