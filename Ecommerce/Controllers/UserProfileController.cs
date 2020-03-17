using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Area("User")]
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}