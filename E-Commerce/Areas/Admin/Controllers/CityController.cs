using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CityController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cities.ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.States = new SelectList(await _context.States.ToListAsync(), "Id", "Name");

            var city = await _context.Cities.SingleOrDefaultAsync(b => b.Id == id);
            if (city != null)
            {
                return PartialView("AddEdit", city);
            }

            return PartialView("AddEdit", new City());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, City city, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    await _context.Cities.AddAsync(city);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.Cities.Update(city);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            ViewBag.States = new SelectList(await _context.States.ToListAsync(), "Id", "Title");

            return PartialView("AddEdit", city);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var city = await _context.Cities.SingleOrDefaultAsync(b => b.Id == id);
            if (city == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", city.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);

                _context.Cities.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

            return RedirectToAction("Index");
        }
    }
}