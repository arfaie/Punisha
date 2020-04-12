using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.Include(o => o.Factor).Include(o => o.Status).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> addIssueCode(string id, string modalId)
        {
            ViewBag.Status = new SelectList(await _context.Statuses.ToListAsync(), "Id", "Title");

            ViewBag.Factor = new SelectList(await _context.Factors.ToListAsync(), "Id", "FactorCode");

            var order = await _context.Orders.Where(o => o.Id == id).SingleOrDefaultAsync();
            if (order != null)
            {
                if (modalId == "1")
                {
                    return PartialView("addIssueCode", order);
                }
                else if (modalId == "2")
                {
                    return PartialView("ChangeStatus", order);
                }

                return null;

            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addIssueCode(string id, Order model, string redirectUrl, string ModalId)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    _context.Orders.Update(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                return null;
            }

            if (ModalId == "1")
            {
                return PartialView("addIssueCode", model);
            }

            return PartialView("ChangeStatus", model);

        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> orderFactor(string id)
        {
            var Select = await _context.Orders.Include(x => x.Factor).ThenInclude(x => x.FactorItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
            return View(Select);
        }
    }
}