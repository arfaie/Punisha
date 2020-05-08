using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.ViewComponents
{
    public class BlogCommentViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BlogCommentViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            HttpContext.Session.SetString("blogId", id);
            return View(await _context.BlogComments.Include(e => e.ApplicationUser).Where(e => e.Accepted == true && e.BlogId == id).ToListAsync());
        }
    }
}
