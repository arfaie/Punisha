using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(await _context.Users.Include(u => u.Car).Include(u => u.UserGroup)
                .FirstOrDefaultAsync(u => u.Id == user.Id));
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var select = await _context.Users.Include(u => u.UserGroup).Include(u => u.Car)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            ViewBag.UserCar = new SelectList(await _context.Cars.Where(c => c.Id == select.CarId).ToListAsync(), "Id", "Name");
            ViewBag.UserMaker = new SelectList(await _context.Makers.ToListAsync(), "Id", "Name");
            ViewBag.Makers = await _context.Makers.OrderBy(m => m.Name).ToListAsync();

            var Cars = await _context.Cars.OrderBy(c => c.Name).ToListAsync();

            foreach (var item in Cars)
            {
                item.Maker = null;
                item.ApplicationUsers = null;
                item.CarProducts = null;
            }

            ViewBag.Cars = Cars;


            return PartialView("EditProfile", select);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ApplicationUser model, string CarId)
        {
            if (ModelState.IsValid)
            {
                model.CarId = CarId;

                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    TempData["Notification"] = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red);
                    return RedirectToAction("Index");
                }

                user.FullName = model.FullName;
                user.NationalCode = model.NationalCode;
                user.Email = model.Email;
                user.CarId = model.CarId;
                user.PhoneNumber = model.PhoneNumber;

                if (user.PhoneNumber != model.PhoneNumber)
                {
                    await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                    user.UserName = model.PhoneNumber;
                    user.Email = model.PhoneNumber + Helper.EmailAddress;
                    user.PhoneNumberConfirmed = true;
                    user.EmailConfirmed = true;
                }

                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePasswordProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return PartialView("ChangePasswordProfile", new AdminChangePasswordViewModel { Id = user.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordProfile(AdminChangePasswordViewModel model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    return RedirectToAction("Index");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    TempData["Notification"] = Notification.ShowNotif("خطایی رخ داد.", ToastType.Red);
                    return PartialView("ChangePasswordProfile", model);
                }

                //await _signInManager.RefreshSignInAsync(user);
                TempData["Notification"] = Notification.ShowNotif("رمز عبور شما با موفقیت ویرایش شد.", ToastType.Green);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            return PartialView("ChangePasswordProfile", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> userAddress()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userAddresses = await _context.Addresses.Where(a => a.UserId == user.Id).Include(a => a.User).Include(a => a.City).ToListAsync();

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            ViewBag.States = await _context.States.OrderBy(x => x.Name).ToListAsync();

            var cities = await _context.Cities.OrderBy(x => x.Name).ToListAsync();

            foreach (var city in cities)
            {
                city.Addresses = null;
                city.Agencies = null;
                city.State = null;
            }

            ViewBag.Cities = cities;

            return View(userAddresses);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEditUserAddress(string id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(c => c.Id == id);

            ViewBag.States = new SelectList(await _context.States.ToListAsync(), "Id", "Name");
            ViewBag.Cities = new SelectList(await _context.Cities.ToListAsync(), "Id", "Name");

            if (address != null)
            {
                return View(address);
            }

            return View(new Address());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditUserAddress(string id, Address model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                model.UserId = user.Id;

                ViewBag.UserFullName = user.FullName;
                ViewBag.UserMobile = user.PhoneNumber;

                if (string.IsNullOrWhiteSpace(id))
                {
                    await _context.Addresses.AddAsync(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                _context.Addresses.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Notification"] = Notification.ShowNotif(MessageType.AddError, ToastType.Red);
            }
            else
            {
                TempData["Notification"] = Notification.ShowNotif(MessageType.EditError, ToastType.Red);
            }

            return View(model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteUserAddress(string id)
        {
            var select = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
            if (select != null)
            {
                _context.Addresses.Remove(select);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
                return RedirectToAction("userAddress");
            }

            return RedirectToAction("userAddress");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> userOrders()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            return View(await _context.Orders.Where(o => o.Factor.UserId == user.Id).Include(o => o.Status).Include(o => o.Factor).OrderByDescending(x => x.TransactionDate).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> orderDetaile(string id, bool isReturned = false)
        {
            ViewBag.path = "/upload/normalimage/";

            ViewBag.barasi = "";
            ViewBag.amadesazi = "";
            ViewBag.post = "";
            ViewBag.moshtari = "";

            ViewBag.isReturned = isReturned;

            var select = await _context.Orders.Include(o => o.Status).Include(o => o.Factor).ThenInclude(o => o.Address)
                .ThenInclude(o => o.City).Include(x => x.Factor).ThenInclude(x => x.FactorItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            switch (select.StatusId)
            {
                case "6f9c65d681937c32dafcec03":
                    ViewBag.barasi = "profile-order-steps-item-active";
                    break;

                case "6f9c65d681937c32dafcec04":
                    ViewBag.amadesazi = "profile-order-steps-item-active";
                    break;

                case "6f9c65d681937c32dafcec05":
                    ViewBag.post = "profile-order-steps-item-active";
                    break;

                case "6f9c65d681937c32dafcec06":
                    ViewBag.moshtari = "profile-order-steps-item-active";
                    break;

                default:
                    ViewBag.barasi = "";
                    ViewBag.amadesazi = "";
                    ViewBag.post = "";
                    ViewBag.moshtari = "";
                    break;
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            return View(select);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> userComments()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            return View(await _context.CommentAndStars.Include(c => c.User).Include(c => c.Product)
                .Where(c => c.UserId == user.Id).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            var histories = await _context.Histories.Where(x => x.UserId == user.Id).OrderByDescending(x => x.RegistrationDateAndTime).ToListAsync();

            var products = new List<Product>();
            foreach (var history in histories)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == history.ProductId);

                if (product != null)
                {
                    products.Add(product);
                }
            }

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> userCommentEdit(string id)
        {
            var select = await _context.CommentAndStars.FirstOrDefaultAsync(c => c.Id == id);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            if (select == null)
            {
                return RedirectToAction("Index");
            }
            return PartialView("userCommentEdit", select);
        }

        [HttpPost]
        public async Task<IActionResult> userCommentEdittt(string id, CommentAndStar model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    _context.CommentAndStars.Update(model);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
                    return PartialView("_SuccessfulResponse", redirectUrl);
                }
            }

            return PartialView("userCommentEdit", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteUserComment(string id)
        {
            var select = await _context.CommentAndStars.FirstOrDefaultAsync(a => a.Id == id);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            if (select != null)
            {
                _context.CommentAndStars.Remove(select);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
                return RedirectToAction("userComments");
            }

            return RedirectToAction("userComments");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ReturnedGoods()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _context.Orders.Where(x => x.Factor.UserId == user.Id && x.Status.Title != "پرداخت نشده").Include(o => o.Status).Include(o => o.Factor).OrderByDescending(x => x.TransactionDate).ToListAsync();

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnRequest(string orderId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red) });
            }

            ViewBag.UserFullName = user.FullName;
            ViewBag.UserMobile = user.PhoneNumber;

            var order = await _context.Orders.Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن سفارش", ToastType.Red) });
            }

            if (order.Status.Title == "تحویل به مشتری" || order.Status.Title == "پرداخت شده" || order.Status.Title == "در صف بررسی" || order.Status.Title == "آماده سازی سفارش" || order.Status.Title == "تحویل به پست")
            {
                var status = await _context.Statuses.FirstOrDefaultAsync(x => x.Title == "درخواست مرجوعی");

                if (status == null)
                {
                    return Json(new { status = "fail", message = Notification.ShowNotif("خطا در ویرایش سفارش", ToastType.Red) });
                }

                order.StatusId = status.Id;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { status = "success", message = Notification.ShowNotif("درخواست مرجوعی برای این سفارش ارسال شد", ToastType.Green) });
            }

            return Json(new { status = "fail", message = Notification.ShowNotif("امکان درخواست مرجوعی برای این سفارش وجود ندارد.", ToastType.Red) });
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(string recipient, string mobile, string phone, string postalCode, string description, string city)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red) });
            }

            var address = new Address
            {
                CityId = city,
                Description = description,
                Recipient = recipient,
                Mobile = mobile,
                Phone = phone,
                PostalCode = postalCode,
                UserId = user.Id
            };

            await _context.Addresses.AddAsync(address);

            try
            {
                await _context.SaveChangesAsync();

                return Json(new { status = "success", message = Notification.ShowNotif("آدرس جدید ثبت شد.", ToastType.Green) });
            }
            catch (Exception e)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در ثبت آدرس", ToastType.Red) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(string id, string recipient, string mobile, string phone, string postalCode, string description, string city)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red) });
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

            if (address == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن آدرس", ToastType.Red) });
            }

            address.CityId = city;
            address.Description = description;
            address.Recipient = recipient;
            address.Mobile = mobile;
            address.Phone = phone;
            address.PostalCode = postalCode;

            _context.Addresses.Update(address);

            try
            {
                await _context.SaveChangesAsync();

                return Json(new { status = "success", message = Notification.ShowNotif("آدرس اصلاح شد.", ToastType.Green) });
            }
            catch (Exception e)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در اصلاح آدرس", ToastType.Red) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddress(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red) });
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

            if (address == null)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در یافتن آدرس", ToastType.Red) });
            }

            _context.Addresses.Remove(address);

            try
            {
                await _context.SaveChangesAsync();

                return Json(new { status = "success", message = Notification.ShowNotif("آدرس حذف شد.", ToastType.Green) });
            }
            catch (Exception e)
            {
                return Json(new { status = "fail", message = Notification.ShowNotif("خطا در حذف آدرس", ToastType.Red) });
            }
        }
    }
}