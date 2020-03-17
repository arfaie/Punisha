using System;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AddressesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iServiceProvider;

        public AddressesController(ApplicationDbContext context, IServiceProvider iServiceProvider)
        {
            _context = context;
            _iServiceProvider = iServiceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var model = await (from addres in _context.Addresses
                               join u in _context.Users on addres.IdUser equals u.Id
                               join c in _context.Cities on addres.IdCity equals c.Id
                               join s in _context.States on c.StatesId equals s.Id
                               select new AddressesViewModel()
                               {
                                   Id = addres.Id,
                                   StateName = s.Name,
                                   IdCity = addres.IdCity,
                                   CityName = c.Name,
                                   Address = addres.Address,
                                   Plaque = addres.Plaque,
                                   PostalCode = addres.PostalCode,
                                   Mobile = addres.Mobile,
                                   Tell = addres.Tell,
                                   Lan = addres.Lan,
                                   Lat = addres.Lat,
                                   IdUser = u.Id,
                                   UserFullName = u.Firstname + " " + u.Lastname
                               }).ToListAsync();

           

            return View(model);
        }



    }
}