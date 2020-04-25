using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.ViewComponents
{
    public class AdminNavbarProductViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AdminNavbarProductViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.rootpath = "/upload/thumbnailimage/";

            return View(await _context.Products.Where(p => p.Inventory <= p.OrderPoint).ToListAsync());
        }
    }
}
