using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce.ViewComponents
{
	public class MainSliderViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;

		public MainSliderViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.Sliders.ToListAsync());
		}
	}
}