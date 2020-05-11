using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class QusetionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QusetionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Qusetions()
        {
            return View(await _context.Questions.Where(q => q.Accepted == true).ToListAsync());
        }

        public async Task<IActionResult> QusetionSearch(string Title)
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                return View("Qusetions",
                    await _context.Questions.Where(q => q.Questions.Contains(Title) && q.Accepted == true).ToListAsync());
            }

            return RedirectToAction("Qusetions");
        }

        [HttpPost]
        public async Task<IActionResult> AddQusetion(string Questions, string Email, string Phone)
        {
            if (!string.IsNullOrWhiteSpace(Questions))
            {
                var question = new Question
                {
                    Questions = Questions,
                    Email = Email,
                    PhoneNumber = Phone,
                    Answer = "",
                    Accepted = false
                };

                _context.Questions.Add(question);

                try
                {
                    TempData["Notification"] = Notification.ShowNotif("پرسش شما ثبت شد", ToastType.Green);
                    await _context.SaveChangesAsync();

                    return Json(new { status = "success", message = Notification.ShowNotif("پرسش شما ثبت شد.", ToastType.Green) });
                }
                catch (Exception e)
                {
                    TempData["Notification"] = Notification.ShowNotif("خطا در ثبت پرسش", ToastType.Red);
                    return Json(new { status = "fail", message = Notification.ShowNotif("خطا در ثبت پرسش", ToastType.Red) });
                }
            }

            TempData["Notification"] = Notification.ShowNotif("خطا در ثبت پرسش", ToastType.Red);
            return Json(new { status = "fail", message = Notification.ShowNotif("خطا در ثبت پرسش", ToastType.Red) });

        }
    }
}