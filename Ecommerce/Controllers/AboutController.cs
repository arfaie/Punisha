using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Ecommerce.Controllers
{
	public class AboutController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceProvider _serviceProvider;

		public AboutController(ApplicationDbContext context, IServiceProvider serviceProvider)
		{
			_context = context;
			_serviceProvider = serviceProvider;
		}

		public IActionResult Index()
		{
			var selectCategories = _context.Categories.ToList();
			string Categories = JsonConvert.SerializeObject(selectCategories);
			HttpContext.Session.SetString("Categories", Categories);

			return View();
		}
	}
}