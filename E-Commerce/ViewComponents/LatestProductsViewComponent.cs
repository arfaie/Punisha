using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
	public class LatestProductsViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;

		public LatestProductsViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.Products.OrderByDescending(x => x.AddingDateTime).ToListAsync());
		}
	}
}