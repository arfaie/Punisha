using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
	public class ProductsListingViewComponent : ViewComponent
	{
		//private readonly ApplicationDbContext _context;

		public ProductsListingViewComponent()
		{
		}

		public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
		{
			return View(products);
		}
	}
}