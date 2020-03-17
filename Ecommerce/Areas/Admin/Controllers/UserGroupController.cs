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
    public class UserGroupController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly IServiceProvider _serviceProvider;

        public UserGroupController(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _Context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _Context.UserGroups.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEditUserGroup(int Id)
        {
            var UserGroup = new UserGroup();
            if (Id != 0)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    UserGroup = await _Context.UserGroups.Where(c => c.Id == Id).SingleOrDefaultAsync();
                    if (UserGroup == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("AddEditUserGroup", UserGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditUserGroup(int Id, UserGroup model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                if (Id == 0)
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.UserGroups.Add(model);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirecturl);
                }
                else
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.UserGroups.Update(model);
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

                return PartialView("AddEditUserGroup", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUserGroup(int Id)
        {
            var UserGroup = new UserGroup();
            if (Id != 0)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    UserGroup = await _Context.UserGroups.Where(c => c.Id == Id).SingleOrDefaultAsync();
                    if (UserGroup == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteUserGroup", UserGroup.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserGroup(int id, string redirecturl)
        {
            if (ModelState.IsValid)
            {

                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await db.UserGroups.Where(c => c.Id == id).SingleOrDefaultAsync();
                    db.UserGroups.Remove(model);
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