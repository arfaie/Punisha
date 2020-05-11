using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ViewComponents
{
    public class AddBlogCommentViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AddBlogCommentViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            return View(new BlogComment());
        }
    }
}
