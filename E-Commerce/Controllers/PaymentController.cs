﻿using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Helpers.ZarinPal;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
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

				var factors = await _context.Factors.Include(x => x.FactorItems).Where(x => !x.IsPaid && x.UserId == user.Id).OrderByDescending(x => x.Date).ToListAsync();

				// check if there is an unpaid factor with the same items
				var factor = factors.FirstOrDefault(x => cartItems.All(x.FactorItems.Select(y => y.ProductId).Contains) && x.FactorItems.Count == cartItems.Count);

				if (factor == null)
				{
					factor = new Factor
					{
						Date = DateTime.UtcNow,
						FactorItems = new List<FactorItem>(),
						UserId = user.Id,
					};

					// generate random factor code
					var allFactorCodes = await _context.Factors.Select(x => x.FactorCode).ToListAsync();

					var random = new Random();
					do
					{
						factor.FactorCode = $"CBT-{random.Next(1234567, 10000000)}";
					} while (allFactorCodes.Contains(factor.FactorCode));

					await using var transaction = _context.Database.BeginTransaction();
					try
					{
						_context.Factors.Add(factor);
						await _context.SaveChangesAsync();

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

						_context.Factors.Update(factor);
						await _context.SaveChangesAsync();

						transaction.Commit();
					}
					catch (Exception)
					{
						transaction.Rollback();
					}
				}
				else
				{
					factor.TotalPrice = factor.FactorItems.Sum(x => x.UnitCount * x.UnitPrice);
					factor.Date = DateTime.UtcNow;

					_context.Factors.Update(factor);
					await _context.SaveChangesAsync();
				}

				return View(factor);
			}

			return View(new Factor());
		}

		//[HttpPost]
		//public async Task<IActionResult> CartAsync(Factor factor)
		//{
		//	factor.FactorItems = await _context.FactorItems.Where(x => x.FactorId == factor.Id).ToListAsync();

		//	factor.TotalPrice = factor.FactorItems.Sum(x => x.UnitCount * x.UnitPrice);

		//	return RedirectToAction("PaymentCheckout", new { id = factor.Id });
		//}

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

			var addresses = await _context.Addresses.Where(x => x.UserId == user.Id).Include(x => x.City).ToListAsync();

			if (addresses.Count > 0)
			{
				factor.AddressId = addresses.ElementAt(0).Id;
			}

			ViewBag.Addresses = addresses;

			if (factor.FactorItems?.Count > 0)
			{
				factor.TotalPrice = factor.FactorItems.Sum(x => x.UnitCount * x.UnitPrice);

				_context.Factors.Update(factor);
				await _context.SaveChangesAsync();

				var productIds = factor.FactorItems.Select(x => x.ProductId).ToList();
				ViewBag.Products = await _context.Products.Where(x => productIds.Contains(x.Id)).ToListAsync();
			}

			return View(factor);
		}

		[HttpPost]
		public async Task<IActionResult> PaymentCheckout(Factor model)
		{
			var factor = await _context.Factors.FirstOrDefaultAsync(x => x.Id == model.Id);

			if (factor != null)
			{
				if (String.IsNullOrWhiteSpace(model.AddressId))
				{
					var user = await _userManager.GetUserAsync(HttpContext.User);

					if (user == null)
					{
						return RedirectToAction("Login", "Account");
					}

					ViewBag.User = user;

					ViewBag.Addresses = await _context.Addresses.Where(x => x.UserId == user.Id).Include(x => x.City).ToListAsync();

					if (factor.FactorItems?.Count > 0)
					{
						factor.TotalPrice = factor.FactorItems.Sum(x => x.UnitCount * x.UnitPrice);

						_context.Factors.Update(factor);
						await _context.SaveChangesAsync();

						var productIds = factor.FactorItems.Select(x => x.ProductId).ToList();
						ViewBag.Products = await _context.Products.Where(x => productIds.Contains(x.Id)).ToListAsync();
					}

					ModelState.AddModelError(String.Empty, "داشتن آدرس تحویل الزامی است.");
					return View(factor);
				}

				factor.ShippingCost = Helper.CalculateShippingCost(_context, model.AddressId);

				factor.AddressId = model.AddressId;
				_context.Factors.Update(factor);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("PaymentConnect", new { id = model.Id });
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
				ModelState.AddModelError(String.Empty, "خطا در محاسبه مبلغ فاکتور");
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
				return RedirectToAction("FailedPayment", new { factorCode = "", error = "خطا در پرداخت" });
			}

			var authority = collection["Authority"];

			var factor = await _context.Factors.FirstOrDefaultAsync(x => x.Id == id);

			if (factor == null)
			{
				return RedirectToAction("FailedPayment", new { factorCode = factor.FactorCode, error = "خطا در تایید فاکتور" });
			}

			TempData["FactorCode"] = factor.FactorCode;

			if (factor.TotalPrice == 0)
			{
				return RedirectToAction("FailedPayment", new { factorCode = factor.FactorCode, error = "خطا در مبلغ درست پرداختی" });
			}

			var verificationResponse = ZarinPalPayment.Verify(factor.TotalPrice, authority);

			if (!verificationResponse.IsSuccess)
			{
				return RedirectToAction("FailedPayment", new { factorCode = factor.FactorCode, error = "خطا در تایید پرداخت" });
			}

			var user = await _userManager.GetUserAsync(HttpContext.User);

			if (user == null)
			{
				return RedirectToAction("FailedPayment", new { factorCode = factor.FactorCode, error = "خطا در تایید کاربر" });
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
					return RedirectToAction("FailedPayment", new { factorCode = factor.FactorCode, error = ex.Message });
				}
			}

			return RedirectToAction("SuccessfulPayment", new { factorCode = factor.FactorCode });
		}

		[HttpGet]
		public IActionResult FailedPayment(string factorCode, string error)
		{
			ViewBag.FactorCode = factorCode;

			ViewBag.StatusMessage = error;

			return View();
		}

		[HttpGet]
		public IActionResult SuccessfulPayment(string factorCode)
		{
			ViewBag.FactorCode = factorCode;

			return View();
		}
	}
}