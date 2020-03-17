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
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _IServiceProvider;

        public BrandController(ApplicationDbContext context,IServiceProvider IServiceProvider)
        {
            _context = context;
            _IServiceProvider = IServiceProvider;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _context.Brands.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddEditBrand(int id)
        {
            var Brand = new Brand();
            if(id !=0)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    Brand = await _context.Brands.Where(b => b.Id == id).SingleOrDefaultAsync();
                    if(Brand==null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("AddEditBrand", Brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditBrand(int Id,Brand model, string redirecturl)
        {
            if(ModelState.IsValid)
            {
                if(Id==0)
                {
                   using(var db=_IServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Brands.Add(model);
                        await db.SaveChangesAsync();
                    }
                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type:ToastType.green);
                    return PartialView("_Succefullyresponse", redirecturl);
                }
                else
                {
                    using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.Brands.Update(model);
                        await db.SaveChangesAsync();
                    }
                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);
                    return PartialView("_Succefullyresponse", redirecturl);
                }
            }
            else
            {
                if(Id==0)
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.addError, type: ToastType.yellow);

                }
                else
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
                }
            }

            return PartialView("AddEditBrand", model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBrand(int Id)
        {
            var Brand = new Brand();
            if(Id!=0)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    Brand = await _context.Brands.Where(b => b.Id == Id).SingleOrDefaultAsync();
                    if(Brand==null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return PartialView("DeleteBrand",Brand.Title);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBrand(int Id, string redirecturl)
        {
            if(ModelState.IsValid)
            {
                using (var db = _IServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await _context.Brands.Where(b => b.Id == Id).SingleOrDefaultAsync();
                    db.Brands.Remove(model);
                  await db.SaveChangesAsync();
                }
                TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);
                return PartialView("_Succefullyresponse", redirecturl);
            }
            else
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type:ToastType.red);
                return RedirectToAction("Index");
            }

        }
    }
}