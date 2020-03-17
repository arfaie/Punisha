using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public IActionResult peyment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Payment()
        {
            int FinalPrice = Convert.ToInt32(TempData["Total"]);
            if (FinalPrice == 0)
            {
                ModelState.AddModelError("AmountError", "مبلغ نمی تواند خالی باشد.لطفا مبلغی را بیشتر از 100 تومان وارد نمایید.");
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Factor", "Factor");
            }

            var payment = await new Zarinpal.Payment("594f1d08-5bfe-11ea-a1d1-000c295eb8fc", Convert.ToInt32(FinalPrice)).PaymentRequest(" ",
                Url.Action(nameof(PaymentVerify), "Payment", new
                {
                    amount = FinalPrice,
                    description = " ",
                    email = " ",
                    mobile = " "
                }, Request.Scheme), " ", " ");

            return payment.Status == 100 ? (IActionResult)Redirect(payment.Link) :
                BadRequest($"خطا در پرداخت. کد خطا:  {payment.Status} ");
        }

        public async Task<IActionResult> PaymentVerify(int amount, string description, string Email, string mobile,
            string Authority, string Status)
        {
            if (Status == "NOK") return View("faildPay");

            var verification = await new ZarinpalSandbox.Payment(amount).Verification(Authority);

            if (verification.Status != 100) return View("faildPay");

            var refId = verification.RefId;

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

            using (var database=_context)
            {
                using (var transaction=database.Database.BeginTransaction())
                {
                    try
                    {
                        Order p = new Order();
                        
                        p.Des = description;
                        p.TransactionNUmber = verification.RefId;
                        p.IdStatus = 1;
                        p.TransactionDate = Convert.ToDateTime(DateTimes);
                        p.IssuCode = 1;
                        int Idfactor = (int)HttpContext.Session.GetInt32("F#%&I!^8@6od");
                        p.IdFactor = Idfactor;

                        database.Orders.Add(p);

                        Factor selectfactor = _context.Factors.Where(f => f.Id == Idfactor).SingleOrDefault();

                        selectfactor.IsPayed = true;

                        database.Factors.Update(selectfactor);

                        database.SaveChanges();

                        transaction.Commit();


                    }
                    catch
                    {
                        
                    }
                }
            }

            return View("SuccefullyPayment");
        }
    }
}