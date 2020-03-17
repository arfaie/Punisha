using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Models;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MessageType = Ecommerce.Helpers.OptionEnums.MessageType;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iServiceProvider;

        public CategoryController(ApplicationDbContext context, IServiceProvider iServiceProvider)
        {
            _context = context;
            _iServiceProvider = iServiceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Categories.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditCategory(int id)
        {
            var category = new category();
            if (id != 0)
            {
                using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    category = await _context.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();
                    if (category == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("AddEditCategory", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditCategory(int id, category model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Categories.Add(model);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
                else
                {
                    using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Categories.Update(model);
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

                return PartialView("AddEditCategory", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = new category();
            if (id != 0)
            {
                using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    category = await _context.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();
                    if (category == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteCategory", category.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await _context.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();
                    db.Categories.Remove(model);
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