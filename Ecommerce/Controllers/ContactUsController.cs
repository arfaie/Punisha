using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Areas.Helpers;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iserviceProvider;
        public ContactUsController(ApplicationDbContext context, IServiceProvider iserviceProvider)
        {
            _context = context;
            _iserviceProvider = iserviceProvider;
        }
        public IActionResult Index()
        {
            var selectCategories = _context.Categories.ToList();
            string Categories = JsonConvert.SerializeObject(selectCategories);
            HttpContext.Session.SetString("Categories", Categories);

            return View();
        }
    }
}