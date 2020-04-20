using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			ViewBag.Makers = await _context.Makers.OrderBy(x => x.Name).ToListAsync();

			var cars = await _context.Cars.OrderBy(x => x.Name).ToListAsync();

			foreach (var car in cars)
			{
				car.Maker = null;
				car.CarProducts = null;
				car.ApplicationUsers = null;
			}

			ViewBag.Cars = cars;
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();

			return View(await Helper.GetAllProductsWithOfferAsync(_context, user));
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}

		public IActionResult Terms()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
	}
}