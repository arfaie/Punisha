using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
	public class MegaMenuViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;

		public MegaMenuViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.CategoryGroups.Include(x => x.Categories).OrderBy(x => x.Title).ToListAsync());
		}
	}
}