using Ecommerce.Models;
using Ecommerce.Models.Helpers;
using Ecommerce.Models.Helpers.OptionEnums;
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
			var query_fullName =
				(from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
				.SingleOrDefault();
			ViewBag.fullname = query_fullName.Firstname + " " + query_fullName.Lastname;

			var select = _context.Cars.ToList();

			ViewBag.Cars = select;

			var select2 = _context.Categories.ToList();

			ViewBag.Categories = select2;

			return View(products);
		}

		[HttpPost]
		public async Task<IActionResult> Factor(string b)// اسم گذاری اشکال داره
		{
			var TotalPrice = TempData["Total"]; // این اطلاعات باید سمت سرور handle بشه، نه سمت کاربر
			var idproducts_ = TempData["IdPoducts"]; // اسم گذاری اشکال داره
			int[] a = (int[])idproducts_; // تبدیل اشکال داره، // اسم گذاری اشکال داره، نوع متغیر انخاب شده اشکال دارا
			List<Product> product = new List<Product>(); // اسم گذاری اشکال داره
			foreach (var VARIABLE in a)// اسم گذاری اشکال داره، اصلا احتیاج به حلفه نیست یه
									   // Linq ساده میشه زد و محصولاتو در آورد
			{
				var select = _context.Products.Where(p => p.Id == VARIABLE).FirstOrDefault(); // استفاده نادرست از linq
				product.Add(select); // امکان خطای نول چک نشده
			}

			Models.Factor factor = new Factor();// اسم گذاری اشکال داره، محل تعریف این متغیر اشکال داره
												// چون بیرون از {} ارزشی نداره
			using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>()) // باید دیتابیس رو ارسال کرد در
																						 // constructor پروژه نه اینکه کل _serviceProvider رو فرستاد
			{
				factor.TotalPrice = Convert.ToDecimal(TotalPrice); // انتخاب نوع decimal برای پول خارجی به درد میخوره
																   // برای تومان یا ریال Integer کافیه چون هیجوقت خرده ای نداره
				factor.TotalDisCount = product.Sum(p => p.Price) - product.Sum(p => p.OldPrice);
				factor.Tax = Convert.ToDecimal(0.09);
				factor.FinalPrice = (Convert.ToDecimal(TotalPrice) - factor.TotalDisCount);
				factor.IdUser = _userManager.GetUserId(User);
				string factorcode = Guid.NewGuid().ToString().Remove(0, 10);// اسم گذاری اشکال داره
																			// استفاده از guid  می تونه به راحتی یه عدد باشه
				factor.FactorCode = factorcode; // اصلا نیازی به تعریف یه متغیر جدید نیسه که خط بعد داره استفاده میشه
				factor.IsPayed = false; // مقدار پیشفرض bool در کلاس خودش همیشه false هست

				var currentDay = DateTime.Now;// باید از UtcNow استفاده بشه که جهانیه نه لوکال
				PersianCalendar pcalender = new PersianCalendar(); // این خطها باید خودش یه تابع عمومی تعریف بشه و هر جا لازمه call بشه
				int year = pcalender.GetYear(currentDay);
				int month = pcalender.GetMonth(currentDay);
				int day = pcalender.GetDayOfMonth(currentDay);
				string ShamsiDate = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(year + "/" + month + "/" + day));

				string GetTime = string.Format("{0:HH:mm:ss}",
					Convert.ToDateTime(currentDay.TimeOfDay.Hours + ":" + currentDay.TimeOfDay.Minutes + ":" +
									   currentDay.TimeOfDay.Seconds));

				string DateTimes = ShamsiDate + " " + GetTime;

				factor.Date = Convert.ToDateTime(DateTimes);

				db.Factors.Add(factor);
				await db.SaveChangesAsync();

				var lastfactorId = _context.Factors.LastOrDefault();

				foreach (var item in a)
				{
					FactorItem factorItem = new FactorItem();
					factorItem.IdFactor = lastfactorId.Id;
					factorItem.IdProduct = item;
					factorItem.Tax = 0;
					var selectproduct = _context.Products.Where(p => p.Id == item).SingleOrDefault();
					factorItem.UnitPrice = selectproduct.Price;
					factorItem.UnitCount = 1;
					factorItem.DisCount = selectproduct.OldPrice - selectproduct.Price;
					factorItem.Tax = 1;
					factorItem.FinalPrice = (selectproduct.OldPrice - selectproduct.Price) * factorItem.Tax;
					factorItem.TotalPrice = Convert.ToDecimal(TotalPrice);

					db.FactorItems.Add(factorItem);
				}

				await db.SaveChangesAsync();
			}
			HttpContext.Session.SetString("F#%&C!^8@6od", factor.FactorCode);
			HttpContext.Session.SetInt32("F#%&I!^8@6od", factor.Id);
			TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);
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
			var query_fullName =
				(from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
				.SingleOrDefault();
			ViewBag.fullname = query_fullName.Firstname + " " + query_fullName.Lastname;

			var select = _context.Cars.ToList();

			ViewBag.Cars = select;

			var select2 = _context.Categories.ToList();

			ViewBag.Categories = select2;

			return View(products);
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> RequestedProduct(int TotalPrice)
		{
			List<Product> product = new List<Product>();
			//foreach (var VARIABLE in IdProducts)
			//{
			//    var select = _context.Products.Where(a => a.Id == VARIABLE).FirstOrDefault();
			//    product.Add(select);
			//}

			Factor factor = new Factor();
			using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
			{
				factor.TotalPrice = TotalPrice;
				factor.TotalDisCount = product.Sum(p => p.OldPrice) - product.Sum(p => p.Price);
				factor.Tax = (decimal)0.09;
				factor.FinalPrice = (factor.TotalDisCount - TotalPrice) * factor.Tax;
				factor.IdUser = _userManager.GetUserId(User);

				var currentDay = DateTime.Now;
				PersianCalendar pcalender = new PersianCalendar();
				int year = pcalender.GetYear(currentDay);
				int month = pcalender.GetMonth(currentDay);
				int day = pcalender.GetDayOfMonth(currentDay);
				string ShamsiDate = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(year + "/" + month + "/" + day));

				string GetTime = string.Format("{0:HH:mm:ss}",
					Convert.ToDateTime(currentDay.TimeOfDay.Hours + ":" + currentDay.TimeOfDay.Minutes + ":" +
									   currentDay.TimeOfDay.Seconds));

				string DateTimes = ShamsiDate + "|" + GetTime;

				factor.Date = Convert.ToDateTime(DateTimes);

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
		public IActionResult DeleteRequestProduct(int Id)
		{
			if (Request.Cookies["S#$51%^Lm*A!2@m"] == null)
			{
				return View("requestedproduct", new List<Product>());
			}

			string cookicontent = Request.Cookies["S#$51%^Lm*A!2@m"].ToString();
			string[] ProductRequestedId = cookicontent.Split(",", StringSplitOptions.RemoveEmptyEntries);
			ProductRequestedId = ProductRequestedId.Where(b => b != "").ToArray();

			List<string> idList = new List<string>(ProductRequestedId);
			idList.Remove(Id.ToString());

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

			var query_fullName = (from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
				.SingleOrDefault();

			ViewBag.fullname = query_fullName.Firstname + " " + query_fullName.Lastname;

			ViewBag.imagepath = "/upload/normalimage/";

			var select = _context.Cars.ToList();

			ViewBag.Cars = select;

			var select2 = _context.Categories.ToList();

			ViewBag.Categories = select2;

			return View("requestedproduct", products);
		}
	}
}