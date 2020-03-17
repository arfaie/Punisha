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
    public class SelectGroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public SelectGroupController(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.SelectGroups.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditSelectGroup(int Id)
        {
            var SelectGroup = new SelectGroup();
            if (Id != 0)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    SelectGroup = await _context.SelectGroups.Where(c => c.Id == Id).SingleOrDefaultAsync();
                    if (SelectGroup == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("AddEditSelectGroup", SelectGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditSelectGroup(int Id, SelectGroup model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                if (Id == 0)
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.SelectGroups.Add(model);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirecturl);
                }
                else
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.SelectGroups.Update(model);
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
                    TempData["Notif"] = Notification.ShowNotif(MessageType.addError, type: ToastType.yellow);
                }
                else
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
                }

                return PartialView("AddEditSelectGroup", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteselectGroup(int Id)
        {
            var selectgroup = new SelectGroup();
            if (Id != 0)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    selectgroup = await _context.SelectGroups.Where(c => c.Id == Id).SingleOrDefaultAsync();
                    if (selectgroup == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteselectGroup", selectgroup.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteselectGroup(int id, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await db.SelectGroups.Where(c => c.Id == id).SingleOrDefaultAsync();
                    db.SelectGroups.Remove(model);
                    await db.SaveChangesAsync();
                }
                TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);

                return PartialView("_Succefullyresponse", redirecturl);
            }
            else
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.yellow);

                return RedirectToAction("Index");
            }

        }
    }
}