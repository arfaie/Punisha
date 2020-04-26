using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.ViewComponents
{
    public class NewsPanelViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public NewsPanelViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.rootpath = "/upload/thumbnailimage/";

            return View(await _context.Newses.Include(n => n.NewCategories).ToListAsync());
        }
    }
}
