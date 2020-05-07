using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
	public class OfferedItemsViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
		{
			return View(products.Where(x => x.DiscountPercent > 0 || x.DiscountPercent > 0).Take(10).ToList());
		}
	}
}