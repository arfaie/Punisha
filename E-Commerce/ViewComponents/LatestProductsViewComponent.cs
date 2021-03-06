﻿using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
    public class LatestProductsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
        {
            return View(products.Where(x => x.IsShow == true).OrderByDescending(x => x.AddingDateTime).Take(10).ToList());
        }
    }
}