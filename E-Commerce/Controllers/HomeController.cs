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
        
        public async Task<IActionResult> getEmail(string strEmail)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    if (!string.IsNullOrEmpty(strEmail))
                    {
                        Email model = new Email();
                        model.strEmail = strEmail;
                        model.Readed = false;

                        await _context.Emails.AddAsync(model);
                        await _context.SaveChangesAsync();

                        return Json(new { status = "success", message = "ایمیل شما با موفقیت ثبت شد"});
                    }
                    else
                    {
                        return Json(new { status = "fail", message = "لطفا ایمیل را صحیح وارد نمایید" });
                    }

                }
                catch (Exception)
                {

                    return Json(new { status = "fail", message = "خطا در ثبت ایمیل" });
                }

            }

            return Json(new { status = "fail", message = "ایمیل وارد شده صحیح نمیباشد" });
        }

    }
}