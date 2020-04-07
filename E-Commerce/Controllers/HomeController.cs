using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
           
            return View();
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

		[Route("error/404")]
		public IActionResult Error404()
		{
			return View();
		}
	}
}