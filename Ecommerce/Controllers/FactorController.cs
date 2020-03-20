using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
        public async Task<IActionResult> Factor(string b)
        {
            var TotalPrice = TempData["Total"];
            var idproducts_ = TempData["IdPoducts"];
            int[] a = (int[])idproducts_;
            List<Product> product = new List<Product>();
            foreach (var VARIABLE in a)
            {
                var select = _context.Products.Where(p => p.Id == VARIABLE).FirstOrDefault();
                product.Add(select);
            }

            Models.Factor factor = new Factor();
            using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
            {

                factor.TotalPrice = Convert.ToDecimal(TotalPrice);
                factor.TotalDisCount = product.Sum(p => p.Price) - product.Sum(p => p.OldPrice);
                factor.Tax = Convert.ToDecimal(0.09);
                factor.FinalPrice = (Convert.ToDecimal(TotalPrice) - factor.TotalDisCount);
                factor.IdUser = _userManager.GetUserId(User);
                string factorcode = Guid.NewGuid().ToString().Remove(0, 10);
                factor.FactorCode = factorcode;
                factor.IsPayed = false;

                var currentDay = DateTime.Now;
                PersianCalendar pcalender = new PersianCalendar();
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
        public IActionResult requestedproduct()
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
        public async Task<IActionResult> requestedproduct(int TotalPrice)
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
            string cookicontent = Request.Cookies["S#$51%^Lm*A!2@m"].ToString();
            string[] ProductRequestedId = cookicontent.Split(",");
            ProductRequestedId = ProductRequestedId.Where(b => b != "").ToArray();

            List<string> idList = new List<string>(ProductRequestedId);
            idList.Remove(Id.ToString());

            cookicontent = "";
            for (int i = 0; i < idList.Count(); i++)
            {
                cookicontent += "," + idList[i] + ",";
            }

            Response.Cookies.Append("S#$51%^Lm*A!2@m", cookicontent,
                new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) });

            ////////////////////////////////////////////////////////////////////////////


            List<Product> products = new List<Product>();


            if (Request.Cookies["S#$51%^Lm*A!2@m"] != null)
            {

                string[] requestedproduct = cookicontent.Split(',');
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

            var query_fullName =
                (from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u)
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