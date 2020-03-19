using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	[Area("User")]
	public class UserProfileController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}