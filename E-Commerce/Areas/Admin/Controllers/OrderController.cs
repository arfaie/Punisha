using System.Linq;
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
        public async Task<IActionResult> addIssueCode(string id)
        {
            ViewBag.Status = new SelectList(await _context.Statuses.ToListAsync(), "Id", "Title");

            ViewBag.Factor = new SelectList(await _context.Factors.ToListAsync(), "Id", "FactorCode");

            var order = await _context.Orders.Where(o => o.Id == id).SingleOrDefaultAsync();
            if (order != null)
            {
                return PartialView("ChangeStatus", order);
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addIssueCode(string id, Order model, string redirectUrl)
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

            return PartialView("ChangeStatus", model);

        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> orderFactor(string id)
        {
            var Select = await _context.Orders.Include(x => x.Factor).ThenInclude(x => x.FactorItems)
                .ThenInclude(x => x.Product).ThenInclude(x => x.Unit).Include(x => x.Factor).ThenInclude(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);
            return View(Select);
        }
    }
}