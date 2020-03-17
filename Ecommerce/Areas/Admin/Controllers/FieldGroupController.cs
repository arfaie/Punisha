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
    public class FieldGroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public FieldGroupController(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }
        
        public async Task<IActionResult> Index()
        {
            var model = await _context.FieldGroups.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditFieldGroup(int id)
        {
            var fieldGroup = new FieldGroup();
            if (id != 0)
            {
                using (_serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    fieldGroup = await _context.FieldGroups.Where(fg => fg.Id == id).SingleOrDefaultAsync();
                    if (fieldGroup == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("AddEditFieldGroup", fieldGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditFieldGroup(int id,FieldGroup model,string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.FieldGroups.Add(model);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
                else
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.FieldGroups.Update(model);
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

                return PartialView("AddEditFieldGroup", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFieldGroup(int id)
        {
            var model = new FieldGroup();
            if (id != 0)
            {
                using (_serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    model = await _context.FieldGroups.Where(fg => fg.Id == id).SingleOrDefaultAsync();
                    if (model == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteFieldGroup", model.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFieldGroup(int id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await _context.FieldGroups.Where(f => f.Id == id).SingleOrDefaultAsync();
                    db.FieldGroups.Remove(model);
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