using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Questions.ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(c => c.Id == id);
            if (question != null)
            {
                return PartialView("AddEdit", question);
            }

            return PartialView("AddEdit", new Question());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, Question model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrWhiteSpace(model.Answer))
                {
                    model.Accepted = true;
                }
                else
                {
                    model.Accepted = false;
                }

                if (String.IsNullOrWhiteSpace(id))
                {
                    _context.Questions.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.Questions.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            return PartialView("AddEdit", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", question.Questions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.Questions.SingleOrDefaultAsync(q => q.Id == id);

                _context.Questions.Remove(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Red);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Accept(string id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(c => c.Id == id);
            if (question != null)
            {
                return PartialView("Accept",question);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(string id, string redirectUrl,string status)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.Questions.SingleOrDefaultAsync(q => q.Id == id);
                if (status == "0")
                {
                    model.Accepted = false;
                }
                else
                {
                    model.Accepted = true;
                }
                
                _context.Questions.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, ToastType.Red);
            return RedirectToAction("Index");
        }
    }
}