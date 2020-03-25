using Ecommerce.Helpers;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
	//FactorController
	public class FactorController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceProvider _serviceProvider;
		private readonly UserManager<ApplicationUser> _userManager;

		public FactorController(ApplicationDbContext context, IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_serviceProvider = serviceProvider;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> Factor()
		{
			List<Product> products = new List<Product>();

			if (Request.Cookies["S#$51%^Lm*A!2@m"] != null)
			{
				string cookicontent = Request.Cookies["S#$51%^Lm*A!2@m"].ToString();
				string[] requestedproduct = cookicontent.Split(',');
				requestedproduct = requestedproduct.Where(r => r != "").ToArray();

				products = (from p in _context.Products
							where requestedproduct.Contains(p.Id.ToString())
							select new Product
							{
								Id = p.Id,
								Name = p.Name,
								Price = (int)p.Price,
								ImageName = p.ImageName,
							}).ToList();
			}

			ViewBag.imagepath = "/upload/normalimage/";
			var queryFullName = (from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
				.SingleOrDefault();
			if (queryFullName != null)
			{
				ViewBag.fullname = queryFullName.Firstname + " " + queryFullName.Lastname;
			}

			ViewBag.Cars = _context.Cars.ToList();

			ViewBag.Categories = _context.Categories.ToList();

			return View(products);
		}

		[HttpPost]
		public async Task<IActionResult> Factor(string b)
		{
			var totalPrice = HttpContext.Session.GetInt32("Total");
			var productIds = HttpContext.Session.GetComplexData<List<int>>("IdPoducts");

			var products = new List<Product>();
			foreach (var productId in productIds)
			{
				var select = _context.Products.FirstOrDefault(p => p.Id == productId);
				products.Add(select);
			}

			using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
			{
				var factor = new Factor
				{
					TotalPrice = Convert.ToDecimal(totalPrice),
					TotalDisCount = products.Sum(p => p.Price) - products.Sum(p => p.OldPrice),
					Tax = Convert.ToDecimal(0.09),
					IdUser = _userManager.GetUserId(User),
					FactorCode = Guid.NewGuid().ToString().Remove(0, 10),
					IsPayed = false
				};

				factor.FinalPrice = (Convert.ToDecimal(totalPrice) - factor.TotalDisCount);

				string dateTimes = Helper.GetPersianDateText(DateTime.Now);

				factor.Date = Convert.ToDateTime(dateTimes);

				db.Factors.Add(factor);
				await db.SaveChangesAsync();

				var lastFactorId = _context.Factors.LastOrDefault();

				foreach (var item in productIds)
				{
					if (lastFactorId != null)
					{
						FactorItem factorItem = new FactorItem
						{
							IdFactor = lastFactorId.Id,
							IdProduct = item,
							Tax = 0
						};

						var selectProduct = _context.Products.SingleOrDefault(p => p.Id == item);
						if (selectProduct != null)
						{
							factorItem.UnitPrice = selectProduct.Price;
							factorItem.UnitCount = 1;
							factorItem.DisCount = selectProduct.OldPrice - selectProduct.Price;
							factorItem.Tax = 1;
							factorItem.FinalPrice = (selectProduct.OldPrice - selectProduct.Price) * factorItem.Tax;
						}

						factorItem.TotalPrice = Convert.ToDecimal(totalPrice);

						db.FactorItems.Add(factorItem);
						await db.SaveChangesAsync();
					}
				}

				HttpContext.Session.SetString("F#%&C!^8@6od", factor.FactorCode);
				HttpContext.Session.SetInt32("F#%&I!^8@6od", factor.Id);
				TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);
			}

			return RedirectToAction("Factor");
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult RequestedProduct()
		{
			List<Product> products = new List<Product>();

			if (Request.Cookies["S#$51%^Lm*A!2@m"] != null)
			{
				string cookicontent = Request.Cookies["S#$51%^Lm*A!2@m"].ToString();
				string[] requestedproduct = cookicontent.Split(',');
				requestedproduct = requestedproduct.Where(r => r != "").ToArray();

				products = (from p in _context.Products
							where requestedproduct.Contains(p.Id.ToString())
							select new Product
							{
								Id = p.Id,
								Name = p.Name,
								Price = (int)p.Price,
								ImageName = p.ImageName,
							}).ToList();
			}

			ViewBag.imagepath = "/upload/normalimage/";
			var queryFullName =
				(from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
				.SingleOrDefault();
			ViewBag.fullname = queryFullName.Firstname + " " + queryFullName.Lastname;

			var select = _context.Cars.ToList();

			ViewBag.Cars = select;

			var select2 = _context.Categories.ToList();

			ViewBag.Categories = select2;

			return View(products);
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> RequestedProduct(int totalPrice)
		{
			var product = new List<Product>();
			//foreach (var VARIABLE in IdProducts)
			//{
			//    var select = _context.Products.Where(a => a.Id == VARIABLE).FirstOrDefault();
			//    product.Add(select);
			//}

			Factor factor = new Factor();
			using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
			{
				factor.TotalPrice = totalPrice;
				factor.TotalDisCount = product.Sum(p => p.OldPrice) - product.Sum(p => p.Price);
				factor.Tax = (decimal)0.09;
				factor.FinalPrice = (factor.TotalDisCount - totalPrice) * factor.Tax;
				factor.IdUser = _userManager.GetUserId(User);

				var currentDay = DateTime.Now;
				PersianCalendar pcalender = new PersianCalendar();
				int year = pcalender.GetYear(currentDay);
				int month = pcalender.GetMonth(currentDay);
				int day = pcalender.GetDayOfMonth(currentDay);
				string shamsiDate = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(year + "/" + month + "/" + day));

				string getTime = string.Format("{0:HH:mm:ss}",
					Convert.ToDateTime(currentDay.TimeOfDay.Hours + ":" + currentDay.TimeOfDay.Minutes + ":" +
									   currentDay.TimeOfDay.Seconds));

				string dateTimes = shamsiDate + "|" + getTime;

				factor.Date = Convert.ToDateTime(dateTimes);

				db.Add(factor);
				await db.SaveChangesAsync();
			}

			TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

			var select = _context.Cars.ToList();

			ViewBag.Cars = select;

			var select2 = _context.Categories.ToList();

			ViewBag.Categories = select2;

			return View();
		}

		[AllowAnonymous]
		[Authorize(Roles = "User")]
		public IActionResult DeleteRequestProduct(int id)
		{
			if (Request.Cookies["S#$51%^Lm*A!2@m"] == null)
			{
				return View("requestedproduct", new List<Product>());
			}

			string cookicontent = Request.Cookies["S#$51%^Lm*A!2@m"].ToString();
			string[] productRequestedId = cookicontent.Split(",", StringSplitOptions.RemoveEmptyEntries);
			productRequestedId = productRequestedId.Where(b => b != "").ToArray();

			List<string> idList = new List<string>(productRequestedId);
			idList.Remove(id.ToString());

			HttpContext.Session.SetInt32("CartItemsCount", idList.Count);

			cookicontent = "";
			for (int i = 0; i < idList.Count(); i++)
			{
				cookicontent += "," + idList[i] + ",";
			}

			if (!String.IsNullOrWhiteSpace(cookicontent))
			{
				Response.Cookies.Append("S#$51%^Lm*A!2@m", cookicontent,
					new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) });
			}
			else
			{
				Response.Cookies.Delete("S#$51%^Lm*A!2@m");
			}

			////////////////////////////////////////////////////////////////////////////

			List<Product> products = new List<Product>();

			if (Request.Cookies["S#$51%^Lm*A!2@m"] != null)
			{
				string[] requestedproduct = cookicontent.Split(',', StringSplitOptions.RemoveEmptyEntries);
				requestedproduct = requestedproduct.Where(r => r != "").ToArray();

				products = (from p in _context.Products
							where requestedproduct.Contains(p.Id.ToString())
							select new Product
							{
								Id = p.Id,
								Name = p.Name,
								Price = p.Price,
								ImageName = p.ImageName,
							}).ToList();
			}

			var queryFullName = (from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
				.SingleOrDefault();

			ViewBag.fullname = queryFullName.Firstname + " " + queryFullName.Lastname;

			ViewBag.imagepath = "/upload/normalimage/";

			var select = _context.Cars.ToList();

			ViewBag.Cars = select;

			var select2 = _context.Categories.ToList();

			ViewBag.Categories = select2;

			return View("requestedproduct", products);
		}
	}
}