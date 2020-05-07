using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
    public class RelatedProductsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RelatedProductsViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null && !String.IsNullOrWhiteSpace(user.CarId))
            {
                ViewBag.Car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == user.CarId);

                var carProductsIds = await _context.CarProducts.Where(x => x.CarId == user.CarId).Select(x => x.ProductId).ToListAsync();

                return View(products.Where(x => x.IsShow == true && carProductsIds.Count > 0 && carProductsIds.Contains(x.Id)).ToList());
            }

            return View();
        }
    }
}