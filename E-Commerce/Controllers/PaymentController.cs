using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Helpers.ZarinPal;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ECommerce.Controllers
{
	[Authorize]
	public class PaymentController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Cart()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			if (user == null)
			{
				return RedirectToAction("Login", "Account");
			}

			if (HttpContext.Session.Keys.Contains("CartItems"))
			{
				var cartItems = HttpContext.Session.GetComplexData<List<string>>("CartItems");
				var products = await _context.Products.Where(x => cartItems.Contains(x.Id)).ToListAsync();

				var factor = new Factor
				{
					Date = DateTime.UtcNow,
					FactorItems = new List<FactorItem>(),
					UserId = user.Id
				};

				await using (var transaction = _context.Database.BeginTransaction())
				{
					try
					{
						_context.Factors.Add(factor);
						//await _context.SaveChangesAsync();

						foreach (var product in products)
						{
							var factorItem = new FactorItem
							{
								FactorId = factor.Id,
								ProductId = product.Id,
								Product = product,
								UnitCount = 1,
								UnitPrice = product.Price
							};

							_context.FactorItems.Add(factorItem);

							factor.FactorItems.Add(factorItem);
						}

						factor.TotalPrice = factor.FactorItems.Sum(x => x.UnitCount * x.UnitPrice);

						await _context.SaveChangesAsync();

						transaction.Commit();
					}
					catch (Exception)
					{
						transaction.Rollback();
					}
				}

				return View(factor);
			}

			return View(new Factor());
		}

		//[HttpPost]
		//public async Task<IActionResult> Cart(string b)
		//{
		//	var totalPrice = HttpContext.Session.GetInt32("Total");
		//	var productIds = HttpContext.Session.GetComplexData<List<int>>("IdPoducts");

		//	var products = new List<Product>();
		//	foreach (var productId in productIds)
		//	{
		//		var select = _context.Products.FirstOrDefault(p => p.Id == productId);
		//		products.Add(select);
		//	}

		//	using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
		//	{
		//		var factor = new Factor
		//		{
		//			TotalPrice = Convert.ToDecimal(totalPrice),
		//			TotalDisCount = products.Sum(p => p.Price) - products.Sum(p => p.OldPrice),
		//			Tax = Convert.ToDecimal(0.09),
		//			IdUser = _userManager.GetUserId(User),
		//			FactorCode = Guid.NewGuid().ToString().Remove(0, 10),
		//			IsPayed = false
		//		};

		//		factor.FinalPrice = (Convert.ToDecimal(totalPrice) - factor.TotalDisCount);

		//		string dateTimes = Helper.GetPersianDateText(DateTime.Now);

		//		factor.Date = Convert.ToDateTime(dateTimes);

		//		db.Factors.Add(factor);
		//		await db.SaveChangesAsync();

		//		var lastFactorId = _context.Factors.LastOrDefault();

		//		foreach (var item in productIds)
		//		{
		//			if (lastFactorId != null)
		//			{
		//				FactorItem factorItem = new FactorItem
		//				{
		//					IdFactor = lastFactorId.Id,
		//					IdProduct = item,
		//					Tax = 0
		//				};

		//				var selectProduct = _context.Products.SingleOrDefault(p => p.Id == item);
		//				if (selectProduct != null)
		//				{
		//					factorItem.UnitPrice = selectProduct.Price;
		//					factorItem.UnitCount = 1;
		//					factorItem.DisCount = selectProduct.OldPrice - selectProduct.Price;
		//					factorItem.Tax = 1;
		//					factorItem.FinalPrice = (selectProduct.OldPrice - selectProduct.Price) * factorItem.Tax;
		//				}

		//				factorItem.TotalPrice = Convert.ToDecimal(totalPrice);

		//				db.FactorItems.Add(factorItem);
		//				await db.SaveChangesAsync();
		//			}
		//		}

		//		HttpContext.Session.SetString("F#%&C!^8@6od", factor.FactorCode);
		//		HttpContext.Session.SetInt32("F#%&I!^8@6od", factor.Id);
		//		TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);
		//	}

		//	return RedirectToAction("Factor");
		//}

		//[AllowAnonymous]
		//[HttpGet]
		//public IActionResult RequestedProduct()
		//{
		//	List<Product> products = new List<Product>();

		//	if (Request.Cookies["S#$51%^Lm*A!2@m"] != null)
		//	{
		//		string cookicontent = Request.Cookies["S#$51%^Lm*A!2@m"].ToString();
		//		string[] requestedproduct = cookicontent.Split(',');
		//		requestedproduct = requestedproduct.Where(r => r != "").ToArray();

		//		products = (from p in _context.Products
		//					where requestedproduct.Contains(p.Id.ToString())
		//					select new Product
		//					{
		//						Id = p.Id,
		//						Name = p.Name,
		//						Price = (int)p.Price,
		//						ImageName = p.ImageName,
		//					}).ToList();
		//	}

		//	ViewBag.imagepath = "/upload/normalimage/";
		//	var queryFullName =
		//		(from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
		//		.SingleOrDefault();
		//	ViewBag.fullname = queryFullName.Firstname + " " + queryFullName.Lastname;

		//	var select = _context.Cars.ToList();

		//	ViewBag.Cars = select;

		//	var select2 = _context.Categories.ToList();

		//	ViewBag.Categories = select2;

		//	return View(products);
		//}

		//[AllowAnonymous]
		//[HttpPost]
		//public async Task<IActionResult> RequestedProduct(int totalPrice)
		//{
		//	var product = new List<Product>();
		//	//foreach (var VARIABLE in IdProducts)
		//	//{
		//	//    var select = _context.Products.Where(a => a.Id == VARIABLE).FirstOrDefault();
		//	//    product.Add(select);
		//	//}

		//	Factor factor = new Factor();
		//	using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
		//	{
		//		factor.TotalPrice = totalPrice;
		//		factor.TotalDisCount = product.Sum(p => p.OldPrice) - product.Sum(p => p.Price);
		//		factor.Tax = (decimal)0.09;
		//		factor.FinalPrice = (factor.TotalDisCount - totalPrice) * factor.Tax;
		//		factor.IdUser = _userManager.GetUserId(User);

		//		var currentDay = DateTime.Now;
		//		PersianCalendar pcalender = new PersianCalendar();
		//		int year = pcalender.GetYear(currentDay);
		//		int month = pcalender.GetMonth(currentDay);
		//		int day = pcalender.GetDayOfMonth(currentDay);
		//		string shamsiDate = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(year + "/" + month + "/" + day));

		//		string getTime = string.Format("{0:HH:mm:ss}",
		//			Convert.ToDateTime(currentDay.TimeOfDay.Hours + ":" + currentDay.TimeOfDay.Minutes + ":" +
		//							   currentDay.TimeOfDay.Seconds));

		//		string dateTimes = shamsiDate + "|" + getTime;

		//		factor.Date = Convert.ToDateTime(dateTimes);

		//		db.Add(factor);
		//		await db.SaveChangesAsync();
		//	}

		//	TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

		//	var select = _context.Cars.ToList();

		//	ViewBag.Cars = select;

		//	var select2 = _context.Categories.ToList();

		//	ViewBag.Categories = select2;

		//	return View();
		//}

		//[AllowAnonymous]
		//[Authorize(Roles = "User")]
		//public IActionResult DeleteRequestProduct(int id)
		//{
		//	if (Request.Cookies["S#$51%^Lm*A!2@m"] == null)
		//	{
		//		return View("requestedproduct", new List<Product>());
		//	}

		//	string cookicontent = Request.Cookies["S#$51%^Lm*A!2@m"].ToString();
		//	string[] productRequestedId = cookicontent.Split(",", StringSplitOptions.RemoveEmptyEntries);
		//	productRequestedId = productRequestedId.Where(b => b != "").ToArray();

		//	List<string> idList = new List<string>(productRequestedId);
		//	idList.Remove(id.ToString());

		//	HttpContext.Session.SetInt32("CartItemsCount", idList.Count);

		//	cookicontent = "";
		//	for (int i = 0; i < idList.Count(); i++)
		//	{
		//		cookicontent += "," + idList[i] + ",";
		//	}

		//	if (!String.IsNullOrWhiteSpace(cookicontent))
		//	{
		//		Response.Cookies.Append("S#$51%^Lm*A!2@m", cookicontent,
		//			new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) });
		//	}
		//	else
		//	{
		//		Response.Cookies.Delete("S#$51%^Lm*A!2@m");
		//	}

		//	////////////////////////////////////////////////////////////////////////////

		//	List<Product> products = new List<Product>();

		//	if (Request.Cookies["S#$51%^Lm*A!2@m"] != null)
		//	{
		//		string[] requestedproduct = cookicontent.Split(',', StringSplitOptions.RemoveEmptyEntries);
		//		requestedproduct = requestedproduct.Where(r => r != "").ToArray();

		//		products = (from p in _context.Products
		//					where requestedproduct.Contains(p.Id.ToString())
		//					select new Product
		//					{
		//						Id = p.Id,
		//						Name = p.Name,
		//						Price = p.Price,
		//						ImageName = p.ImageName,
		//					}).ToList();
		//	}

		//	var queryFullName = (from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
		//		.SingleOrDefault();

		//	ViewBag.fullname = queryFullName.Firstname + " " + queryFullName.Lastname;

		//	ViewBag.imagepath = "/upload/normalimage/";

		//	var select = _context.Cars.ToList();

		//	ViewBag.Cars = select;

		//	var select2 = _context.Categories.ToList();

		//	ViewBag.Categories = select2;

		//	return View("requestedproduct", products);
		//}

		[HttpPost]
		public async Task<IActionResult> Cart(Factor factor)
		{
			return RedirectToAction("PaymentCheckout", new { id = factor.Id });
		}

		[HttpGet]
		public async Task<IActionResult> PaymentCheckout(string id)
		{
			var factor = await _context.Factors.Include(x => x.FactorItems).FirstOrDefaultAsync(x => x.Id == id);

			if (factor == null)
			{
				return RedirectToAction("Error", "Home");
			}

			var user = await _userManager.GetUserAsync(HttpContext.User);

			if (user == null)
			{
				return RedirectToAction("Login", "Account");
			}

			ViewBag.User = user;

			if (factor.FactorItems?.Count > 0)
			{
				var productIds = factor.FactorItems.Select(x => x.ProductId).ToList();
				ViewBag.Products = await _context.Products.Where(x => productIds.Contains(x.Id)).ToListAsync();
			}

			return View(factor);
		}

		[HttpPost]
		public async Task<IActionResult> PaymentCheckout(Factor factor)
		{
			return RedirectToAction("PaymentConnect", new { id = factor.Id });
		}

		[HttpGet]
		public async Task<IActionResult> PaymentConnect(string id)
		{
			var factor = await _context.Factors.Include(x => x.FactorItems).FirstOrDefaultAsync(x => x.Id == id);

			if (factor == null)
			{
				return RedirectToAction("Error", "Home");
			}

			if (factor.FactorItems == null || factor.FactorItems.Count == 0)
			{
				ModelState.AddModelError(String.Empty, $"خطا محاسبه مبلغ فاکتور");
				return View(factor);
			}

			factor.TotalPrice = factor.FactorItems.Sum(x => x.UnitCount * x.UnitPrice);

			if (factor.TotalPrice == 0)
			{
				ModelState.AddModelError("AmountError", "مبلغ نمی تواند خالی باشد. لطفا مبلغی را بیشتر از 100 تومان وارد نمایید.");
				return View(factor);
			}

			_context.Factors.Update(factor);
			await _context.SaveChangesAsync();

			var callbackUrl = $"http://{Request.Host}/Payment/{nameof(PaymentVerify)}/{factor.Id}";

			var description = "پرداخت در سایت کاربیوتیک";

			var response = ZarinPalPayment.Request(factor.TotalPrice, description, callbackUrl);

			// if there is an error show this page again
			if (response.Status == 100)
			{
				Response.Redirect(ZarinPalPayment.GetPaymentGatewayUrl(response.Authority));
			}
			else
			{
				ModelState.AddModelError(String.Empty, $"خطا در پرداخت. کد خطا: {response.Status} ");
			}

			return View(factor);
		}

		public async Task<IActionResult> PaymentVerify(string id)
		{
			var collection = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.Value);
			var status = collection["Status"];

			if (status != "OK")
			{
				ModelState.AddModelError("AmountError", "خطا در پرداخت");

				return View("FailedPayment");
			}

			var authority = collection["Authority"];

			var factor = await _context.Factors.FirstOrDefaultAsync(x => x.Id == id);

			if (factor == null)
			{
				ModelState.AddModelError(String.Empty, "خطا در تایید فاکتور");
				return View("FailedPayment");
			}

			if (factor.TotalPrice == 0)
			{
				ModelState.AddModelError(String.Empty, "خطا در مبلغ درست پرداختی");
				return View("FailedPayment");
			}

			var verificationResponse = ZarinPalPayment.Verify(factor.TotalPrice, authority);

			if (!verificationResponse.IsSuccess)
			{
				ModelState.AddModelError(String.Empty, "خطا در تایید پرداخت");
				return View("FailedPayment");
			}

			var user = await _userManager.GetUserAsync(HttpContext.User);

			if (user == null)
			{
				ModelState.AddModelError(String.Empty, "خطا در تایید کاربر");
				return View("FailedPayment");
			}
			//string dateTimes = Helper.GetPersianDateText(DateTime.Now);

			var statuses = await _context.Statuses.FirstOrDefaultAsync();
			if (statuses == null)
			{
				statuses = new Status
				{
					Title = "پرداخت شده"
				};

				_context.Statuses.Add(statuses);
				await _context.SaveChangesAsync();
			}

			await using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var order = new Order
					{
						Description = user.UserName,
						TransactionNumber = verificationResponse.RefId,
						StatusId = statuses.Id,
						TransactionDate = DateTime.UtcNow,
						IssueCode = 1,
						FactorId = factor.Id,
						TransactionStatus = true
					};

					_context.Orders.Add(order);

					factor.IsPaid = true;
					_context.Factors.Update(factor);

					HttpContext.Session.Remove("CartItems");

					await _context.SaveChangesAsync();

					transaction.Commit();
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(String.Empty, ex.Message);
					return View("FailedPayment");
				}
			}

			return View("SuccessfulPayment");
		}
	}
}