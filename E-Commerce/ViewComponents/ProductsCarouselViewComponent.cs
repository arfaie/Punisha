using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
	public class ProductsCarouselViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
		{
			return View(products);
		}
	}
}