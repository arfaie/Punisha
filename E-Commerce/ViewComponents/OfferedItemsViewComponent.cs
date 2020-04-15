using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
	public class OfferedItemsViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public OfferedItemsViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			if (user != null)
			{
				return View(await Helper.GetProductsWithOfferAsync(_context, user));
			}

			return View();
		}
	}
}