using System;
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
    public class StateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _IserviceProvider;

        public StateController(ApplicationDbContext context, IServiceProvider IserviceProvider)
        {
            _context = context;
            _IserviceProvider = IserviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.States.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditstate(int Id)
        {
            var state = new State();
            if (Id != 0)
            {
                using (var db = _IserviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    state = await _context.States.Where(s => s.Id == Id).SingleOrDefaultAsync();
                    if (state == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("AddEditstate", state);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditstate(int Id, State model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                if (Id == 0)
                {
                    using (var db = _IserviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.States.Add(model);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirecturl);
                }
                else
                {
                    using (var db = _IserviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        db.States.Update(model);
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

                return PartialView("AddEditstate", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Deletestate(int Id)
        {
            var state = new State();
            if (Id != 0)
            {
                using (var db = _IserviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    state = await _context.States.Where(s => s.Id == Id).SingleOrDefaultAsync();
                    if (state == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("Deletestate", state.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletestate(int id, string redirecturl)
        {
            if (ModelState.IsValid)
            {
               
                using (var db = _IserviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await _context.States.Where(s => s.Id == id).SingleOrDefaultAsync();

                    db.States.Remove(model);
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