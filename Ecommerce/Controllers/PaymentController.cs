using Ecommerce.Helpers.ZarinPal;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ecommerce.Controllers
{
	public class PaymentController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> Payment()
		{
			if (HttpContext.Session != null && HttpContext.Session.Keys.Contains("Total"))
			{
				int? finalPrice = HttpContext.Session.GetInt32("Total");
				if (finalPrice == null)
				{
					ModelState.AddModelError(String.Empty, $"خطا محاسبه مبلغ فاکتور");
					return View();
				}

				if (finalPrice.Value == 0)
				{
					ModelState.AddModelError("AmountError", "مبلغ نمی تواند خالی باشد.لطفا مبلغی را بیشتر از 100 تومان وارد نمایید.");
					return View();
				}

				if (!ModelState.IsValid)
				{
					return RedirectToAction("Factor", "Factor");
				}

				var callbackUrl = $"http://{Request.Host}/Payment/{nameof(PaymentVerify)}";

				var description = "پرداخت در سایت کاربیوتیک";

				var response = ZarinPalPayment.Request(finalPrice.Value, description, callbackUrl);

				// if there is an error show this page again
				if (response.Status == 100)
				{
					Response.Redirect(ZarinPalPayment.GetPaymentGatewayUrl(response.Authority));
				}
				else
				{
					ModelState.AddModelError(String.Empty, $"خطا در پرداخت. کد خطا: {response.Status} ");
				}
			}

			return View();

			//var payment = await new Zarinpal.Payment("594f1d08-5bfe-11ea-a1d1-000c295eb8fc", Convert.ToInt32(finalPrice)).PaymentRequest(" ", Url.Action(nameof(PaymentVerify), "Payment", new
			//{
			//	amount = finalPrice,
			//	description = " ",
			//	email = " ",
			//	mobile = " "
			//}, Request.Scheme), " ", " ");

			//return payment.Status == 100 ? (IActionResult)Redirect(payment.Link) :
			//	BadRequest($"خطا در پرداخت. کد خطا:  {payment.Status} ");
		}

		public async Task<IActionResult> PaymentVerify()
		{
			var collection = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.Value);
			var status = collection["Status"];

			if (status != "OK")
			{
				ModelState.AddModelError("AmountError", "خطا در پرداخت");

				return View("FailedPay");
			}

			var authority = collection["Authority"];

			var finalPrice = HttpContext.Session.GetInt32("Total");

			if (!finalPrice.HasValue || finalPrice.Value == 0)
			{
				ModelState.AddModelError(String.Empty, "خطا در مبلغ درست پرداختی");
				return View("FailedPay");
			}

			var verificationResponse = ZarinPalPayment.Verify(finalPrice.Value, authority);

			if (verificationResponse.Status != 100)
			{
				ModelState.AddModelError(String.Empty, "خطا در تایید پرداخت");
				return View("FailedPay");
			}

			var user = await _userManager.GetUserAsync(HttpContext.User);

			if (user == null)
			{
				ModelState.AddModelError(String.Empty, "خطا در تایید کاربر");
				return View("FailedPay");
			}
			//string dateTimes = Helper.GetPersianDateText(DateTime.Now);

			using (_context)
			{
				using (var transaction = _context.Database.BeginTransaction())
				{
					try
					{
						Order p = new Order
						{
							Des = user.UserName,
							TransactionNUmber = Convert.ToInt64(verificationResponse.RefId),
							IdStatus = 1,
							TransactionDate = DateTime.Now, //Convert.ToDateTime(dateTimes),
							IssuCode = 1
						};

						if (HttpContext.Session != null)
						{
							int idFactor = (int)HttpContext.Session.GetInt32("F#%&I!^8@6od");
							p.IdFactor = idFactor;

							_context.Orders.Add(p);

							var selectedFactor = await _context.Factors.SingleOrDefaultAsync(f => f.Id == idFactor);

							selectedFactor.IsPayed = true;

							_context.Factors.Update(selectedFactor);

							HttpContext.Session.SetInt32("CartItemsCount", 0);
						}

						await _context.SaveChangesAsync();

						transaction.Commit();
					}
					catch (Exception ex)
					{
						ModelState.AddModelError(String.Empty, ex.Message);
						return View("FailedPay");
					}
				}
			}

			return View("SuccessfulPayment");
		}
	}
}