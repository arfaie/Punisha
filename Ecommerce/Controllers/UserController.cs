using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Ecommerce.Areas.Helpers;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static Ecommerce.Models.ViewModels.SMSViewModel;

namespace Ecommerce.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _appEnvironment;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IServiceProvider serviceProvider, IHostingEnvironment appEnvironment)
        {
            _serviceProvider = serviceProvider;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Register()
        {

            var select = _context.Cars.ToList();

            ViewBag.Cars = select;

            var select2 = _context.Categories.ToList();

            ViewBag.Categories = select2;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var isExist = _context.Users.Where(x => x.PhoneNumber == model.PhoneNumber).FirstOrDefault();

            if (isExist != null)//کاربری با این شماره موبایل وجود دارد
            {
                TempData["Mobile"] = isExist.PhoneNumber;

                var select = _context.Cars.ToList();

                ViewBag.Cars = select;

                var select2 = _context.Categories.ToList();

                ViewBag.Categories = select2;

                return View("ResendUserPass");
            }
            else
            {
                TempData["Mobile"] = model.PhoneNumber;
                Random rnd = new Random();
                int username;
                do
                {
                    username = rnd.Next(100000, 999999);
                } while (_context.Users.Where(x => x.UserName == username.ToString()).Any());

                ApplicationUser user = new ApplicationUser()
                {
                    Firstname = "",
                    Lastname = "",
                    PhoneNumber = model.PhoneNumber,
                    UserName = username.ToString(),
                    Email = "",
                    IdCar = 1,
                    IdUserGroup = 1,
                    NatCode = 0,
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ApplicationRole approle = await _roleManager.FindByIdAsync("2");
                    if (approle != null)
                    {
                        IdentityResult Roleresult = await _userManager.AddToRoleAsync(user, approle.Name);
                        if (Roleresult.Succeeded)
                        {
                            var select = _context.Users.Where(x => x.PhoneNumber == model.PhoneNumber).First();
                            HttpContext.Session.SetString("idUser", select.Id);

                            var selec5t = _context.Cars.ToList();

                            ViewBag.Cars = selec5t;

                            var select2 = _context.Categories.ToList();

                            ViewBag.Categories = select2;

                            return View("GetNumber");

                        }
                    }
                }
                else
                {
                    var select = _context.Cars.ToList();

                    ViewBag.Cars = select;

                    var select2 = _context.Categories.ToList();

                    ViewBag.Categories = select2;

                    return View("Register");
                }

            }

            var selec555t = _context.Cars.ToList();

            ViewBag.Cars = selec555t;

            var select222 = _context.Categories.ToList();

            ViewBag.Categories = select222;

            return View("Register");
        }
        public IActionResult Login()
        {
            var selec555t = _context.Cars.ToList();

            ViewBag.Cars = selec555t;

            var select222 = _context.Categories.ToList();

            ViewBag.Categories = select222;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string UserName, string Password)
        {
            if (ModelState.IsValid)
            {
                var isExist = _context.Users.Where(x => x.UserName == UserName || x.PhoneNumber == UserName).FirstOrDefault();

                var login = await _signInManager.PasswordSignInAsync(isExist.UserName, Password, true, lockoutOnFailure: false);
                //TempData["Mobile"] = isExist.Mobile;
                if (login.Succeeded)//کاربر وجود دارد
                {
                    UserViewModel ob = new UserViewModel();
                    ob = await UserInfo(isExist.Id);
                    HttpContext.Session.SetString("idUser", isExist.Id);
                    //HttpContext.Session.SetString("PhoneNumber", isExist.Id);
                    if (isExist.PhoneNumberConfirmed)
                    {
                        if (isExist.Firstname == "" || isExist.Lastname == "")//اطلاعات کاربری تکمیل نشده است
                        {
                            TempData["CompelateMsg"] = "اطلاعات کاربری خود را تکمیل نمایید";

                            var selec555t = _context.Cars.ToList();

                            ViewBag.Cars = selec555t;

                            var select222 = _context.Categories.ToList();

                            ViewBag.Categories = select222;

                            return View("EditProfile", ob);
                        }
                        else
                        {
                            //goto Personal page

                            var selec555t = _context.Cars.ToList();

                            ViewBag.Cars = selec555t;

                            var select222 = _context.Categories.ToList();

                            ViewBag.Categories = select222;

                            return View("Profile", ob);
                        }

                    }
                    else
                    {
                        //goto verify mobile
                        TempData["Mobile"] = isExist.PhoneNumber;

                        var selec555t = _context.Cars.ToList();

                        ViewBag.Cars = selec555t;

                        var select222 = _context.Categories.ToList();

                        ViewBag.Categories = select222;

                        return View("GetNumber", isExist.Mobile);

                    }
                }
                else//کاربر وجود ندراد
                {
                    //TempData["isUser"] = false;


                    var selec555t = _context.Cars.ToList();

                    ViewBag.Cars = selec555t;

                    var select222 = _context.Categories.ToList();

                    ViewBag.Categories = select222;

                    return View("GetNumber");

                }              
           }
            var selec55t = _context.Cars.ToList();

            ViewBag.Cars = selec55t;

            var select22 = _context.Categories.ToList();

            ViewBag.Categories = select22;

            return View();
        }

        public async Task<IActionResult> GetNumber(string Mobile)
        {
            var db = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            List<RootObject> EmpInfo = new List<RootObject>();
            Random rnd = new Random();
            int token = rnd.Next(100000, 999999);

            var select = _context.Users.Where(x => x.PhoneNumber == Mobile).FirstOrDefault();
            if (select != null)
            {
                TempData["Mobile"] = select.PhoneNumber;
            }
            HttpContext.Session.SetInt32("token", token);



            if (select != null && select.Firstname != "" && select.Lastname != "" && select.PhoneNumberConfirmed==true)//کاربر با این شماره وجود دارد/موبایل تایید شده/بازیابی رمز عبور و نام کاربری
            {
                var selec555t = _context.Cars.ToList();

                ViewBag.Cars = selec555t;

                var select222 = _context.Categories.ToList();

                ViewBag.Categories = select222;

                return View("ResendUserPass");
            }
            
            else//کاربر با این شماره وجود ندار/ارسال کد تایید
            {
                SendSMS sendSMS = new SendSMS();
                bool send = await sendSMS.SendAsync((int)SMSTypes.Register, Mobile, token.ToString());
                if (send)
                {
                    var selec555t = _context.Cars.ToList();

                    ViewBag.Cars = selec555t;

                    var select222 = _context.Categories.ToList();

                    ViewBag.Categories = select222;

                    return View("VerifyCode");
                }
                else
                {
                    TempData["SMSNotSend"] = "ارسال پیامک با خطا مواجه شد.لطفا مجددا سعی نمایید";

                    var selec555t = _context.Cars.ToList();

                    ViewBag.Cars = selec555t;

                    var select222 = _context.Categories.ToList();

                    ViewBag.Categories = select222;

                    return View("GetNumber");
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(string inputCode)
        {
            #region Check Direct Access
            /*
             if(VerifyCode || Mobile == null)
                Goto NotFoundPage
             */
            #endregion
            int token = (int)HttpContext.Session.GetInt32("token");
            UserViewModel user = new UserViewModel();

            string idUser = HttpContext.Session.GetString("idUser");

            if (idUser != null)//کاربر وجود دارد
            {
                var isUser = _context.Users.Where(x => x.Id == idUser).FirstOrDefault();
                if (inputCode == token.ToString())
                {
                    isUser.PhoneNumberConfirmed = true;
                    _context.Users.Update(isUser);
                    var db = _serviceProvider.GetRequiredService<ApplicationDbContext>();
                    db.SaveChanges();
                    UserViewModel ob = new UserViewModel();
                    ob = await UserInfo(isUser.Id);
                    //goto User Page
                    if (!(isUser.Firstname == "" || isUser.Lastname == ""))//اطلاعات کاربری تکمیل نشده است
                    {
                        var selec555t = _context.Cars.ToList();

                        ViewBag.Cars = selec555t;

                        var select222 = _context.Categories.ToList();

                        ViewBag.Categories = select222;

                        return View("Profile", ob);
                    }
                    else//goto Profile Page>Complete Profile
                    {
                        TempData["CompelateMsg"] = "اطلاعات کاربری خود را تکمیل نمایید";

                        var selec555t = _context.Cars.ToList();

                        ViewBag.Cars = selec555t;

                        var select222 = _context.Categories.ToList();

                        ViewBag.Categories = select222;

                        return View("EditProfile", ob);
                    }
                }
                else
                {
                    TempData["InvalidCode"] = "کد وارد شده صحیح نمیباشد";

                    var selec555t = _context.Cars.ToList();

                    ViewBag.Cars = selec555t;

                    var select222 = _context.Categories.ToList();

                    ViewBag.Categories = select222;

                    return View();
                }
            }
            else//کاربر جدید است
            {
                if (inputCode == token.ToString())
                {
                    UserViewModel ob = new UserViewModel();

                    var selec555t = _context.Cars.ToList();

                    ViewBag.Cars = selec555t;

                    var select222 = _context.Categories.ToList();

                    ViewBag.Categories = select222;
                    return View("EditProfile", ob);
                }
            }

            var selec55t = _context.Cars.ToList();

            ViewBag.Cars = selec55t;

            var select22 = _context.Categories.ToList();

            ViewBag.Categories = select22;

            return View();
        }
        public IActionResult ResendUserPass(bool Get = true)
        {
            var selec555t = _context.Cars.ToList();

            ViewBag.Cars = selec555t;

            var select222 = _context.Categories.ToList();

            ViewBag.Categories = select222;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResendUserPass(string PhoneNumber)
        {
            SendSMS sendSMS = new SendSMS();
            var select = _context.Users.Where(x => x.PhoneNumber == PhoneNumber).FirstOrDefault();
            Utilities ut = new Utilities();
            string NewPass = ut.Random(100000, 999999).ToString();

            //Reset PassWord//
            String userId = select.Id;

            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_context);

            ApplicationUser cUser = await store.FindByIdAsync(userId);

            String newPassword = NewPass;

            String hashedNewPassword = _userManager.PasswordHasher.HashPassword(cUser, newPassword);


            await store.SetPasswordHashAsync(cUser, hashedNewPassword);
            await store.UpdateAsync(cUser);
            //Reset PassWord//

            //Resend User and Pass
            bool send = await sendSMS.SendAsync((int)SMSTypes.ResendUserPass, PhoneNumber, select.UserName, NewPass);
            if (send)
            {
                TempData["ResenUserPass_Seccecful"] = "اطلاعات حساب کاربری با موفقیت ارسال گردید";

                var selec555t = _context.Cars.ToList();

                ViewBag.Cars = selec555t;

                var select222 = _context.Categories.ToList();

                ViewBag.Categories = select222;

                return View("Login");
            }
            else
            {
                TempData["SMSNotSend"] = "ارسال پیامک با خطا مواجه شد.لطفا مجددا سعی نمایید";

                var selec555t = _context.Cars.ToList();

                ViewBag.Cars = selec555t;

                var select222 = _context.Categories.ToList();

                ViewBag.Categories = select222;

                return View("GetNumber");
            }
            //Resend User and Pass



        }

        public async Task<IActionResult> Profile()
        {

            string idUser = HttpContext.Session.GetString("idUser");
            UserViewModel ob = new UserViewModel();
            ob = await UserInfo(idUser);
            var model = _context.Users.Where(x => x.Id == idUser).FirstOrDefault();
            ob = await UserInfo(idUser);
            if (!(model.Firstname == "" || model.Lastname == ""))//اطلاعات کاربری تکمیل نشده است
            {
                var selec555t = _context.Cars.ToList();

                ViewBag.Cars = selec555t;

                var select222 = _context.Categories.ToList();

                ViewBag.Categories = select222;

                return View(ob);
            }
            else
            {
                TempData["CompelateMsg"] = "اطلاعات کاربری خود را تکمیل نمایید";

                var selec555t = _context.Cars.ToList();

                ViewBag.Cars = selec555t;

                var select222 = _context.Categories.ToList();

                ViewBag.Categories = select222;

                return View("EditProfile", ob);
            }

        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            UserViewModel ob = new UserViewModel();
            string idUser = HttpContext.Session.GetString("idUser");
            if (idUser != null)
            {
                ob = await UserInfo(idUser);

                var selec555t = _context.Cars.ToList();

                ViewBag.Cars = selec555t;

                var select222 = _context.Categories.ToList();

                ViewBag.Categories = select222;

                return View(ob);
            }
            else
            {

                var selec555t = _context.Cars.ToList();

                ViewBag.Cars = selec555t;

                var select222 = _context.Categories.ToList();

                ViewBag.Categories = select222;

                return View("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserViewModel model)
        {
            ApplicationUser ob = new ApplicationUser();
            string idUser = HttpContext.Session.GetString("idUser");

            ob = _context.Users.Where(x => x.Id == idUser).First();
            ob.Firstname = model.FirstName;
            ob.Lastname = model.LastName;
            ob.NatCode = model.Natcode;
            ob.PhoneNumber = model.PhoneNumber;
            ob.Email = model.Email;
            ob.IdCar = model.CarId;
            _context.Users.Update(ob);
            var db = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            db.SaveChanges();

            var selec555t = _context.Cars.ToList();

            ViewBag.Cars = selec555t;

            var select222 = _context.Categories.ToList();

            ViewBag.Categories = select222;

            return View("Profile", model);
        }

        public async Task<UserViewModel> UserInfo(string id)
        {
            var select = _context.Users.Where(x => x.Id == id).First();
            UserViewModel ob = new UserViewModel();
            ob.FirstName = select.Firstname;
            ob.LastName = select.Lastname;
            ob.Natcode = select.NatCode;
            ob.PhoneNumber = select.PhoneNumber;
            ob.Email = select.Email;
            ob.CarId = (int)select.IdCar;


            ob.CarListItems = await _context.Cars.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();

            var selec555t = _context.Cars.ToList();

            ViewBag.Cars = selec555t;

            var select222 = _context.Categories.ToList();

            ViewBag.Categories = select222;

            return ob;
        }

        public async Task<ActionResult> Logout()
        {
            if (Request.Cookies["S#$51%^Lm*A!2@m"] != null)
            {
                Response.Cookies.Delete("S#$51%^Lm*A!2@m");
            }
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}