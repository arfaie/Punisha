using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
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

            ViewBag.Cars = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name");
            ViewBag.Makers = new SelectList(await _context.Makers.ToListAsync(), "Id", "Name");

            return PartialView("EditProfile", select);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    TempData["Notification"] = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red);


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

    }
}