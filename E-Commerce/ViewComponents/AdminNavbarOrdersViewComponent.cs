using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.ViewComponents
{
    public class AdminNavbarOrdersViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AdminNavbarOrdersViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Orders.Where(e => e.StatusId == "6f9c65d681937c32dafcec03").ToListAsync());
        }
    }
}

