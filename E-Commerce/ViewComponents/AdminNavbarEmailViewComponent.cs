using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.ViewComponents
{
    public class AdminNavbarEmailViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AdminNavbarEmailViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Emails.Where(e => e.Readed == false).ToListAsync());
        }

    }
}
