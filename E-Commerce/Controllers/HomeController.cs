using System;
using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.Makers = await _context.Makers.OrderBy(x => x.Name).ToListAsync();

            var cars = await _context.Cars.OrderBy(x => x.Name).ToListAsync();

            foreach (var car in cars)
            {
                car.Maker = null;
                car.CarProducts = null;
                car.ApplicationUsers = null;
            }

            ViewBag.Cars = cars;
            ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
            ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();

            return View(await Helper.GetAllProductsWithOfferAsync(_context, user));
        }

        public async Task<IActionResult> About()
        {
            return View(await _context.AboutUses.ToListAsync());
        }

        public async Task<IActionResult> Contact()
        {
            return View(await _context.AboutUses.ToListAsync());
        }

        public IActionResult ContactSlider1()
        {
            return View();
        }

        public IActionResult ContactSlider2()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> getEmail(string strEmail)
        {
            if (ModelState.IsValid)
            {
                Email model = new Email();
                model.strEmail = strEmail;
                model.Readed = false;

                await _context.Emails.AddAsync(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Qusetions()
        {
            return View(await _context.Questions.Where(q => q.Accepted == true).ToListAsync());
        }

        public async Task<IActionResult> QusetionSearch(string Title)
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                return View("Qusetions",
                    await _context.Questions.Where(q => q.Questions.Contains(Title) && q.Accepted == true).ToListAsync());
            }

            return RedirectToAction("Qusetions");
        }

        [HttpPost]
        public async Task<IActionResult> AddQusetion(string Questions, string Email, string Phone)
        {
            if (!string.IsNullOrWhiteSpace(Questions))
            {
                var question = new Question
                {
                    Questions = Questions,
                    Email = Email,
                    PhoneNumber = Phone,
                    Answer = "",
                    Accepted = false
                };

                _context.Questions.Add(question);

                try
                {
                    TempData["Notification"] = Notification.ShowNotif("پرسش شما ثبت شد", ToastType.Green);
                    await _context.SaveChangesAsync();

                    return Json(new { status = "success", message = Notification.ShowNotif("پرسش شما ثبت شد.", ToastType.Green) });
                }
                catch (Exception e)
                {
                    TempData["Notification"] = Notification.ShowNotif("خطا در ثبت پرسش", ToastType.Red);
                    return Json(new { status = "fail", message = Notification.ShowNotif("خطا در ثبت پرسش", ToastType.Red) });
                }
            }

            TempData["Notification"] = Notification.ShowNotif("خطا در ثبت پرسش", ToastType.Red);
            return Json(new { status = "fail", message = Notification.ShowNotif("خطا در ثبت پرسش", ToastType.Red) });

        }

        [HttpPost]
        public async Task<IActionResult> AddCommentBlog(BlogComment model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            model.UserId = user.Id;
            string blogid = HttpContext.Session.GetString("BlogId");
            HttpContext.Session.SetString("BlogId", "");
            model.BlogId = blogid;

            _context.BlogComments.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("DetailesBlog", "Blog", blogid);
        }
    }
}