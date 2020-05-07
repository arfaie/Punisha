using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
    public class BestSaleProductsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
        {
            return View(products.Where(p => p.IsShow == true).OrderByDescending(x => x.FactorItems?.Count).ToList());
        }
    }
}