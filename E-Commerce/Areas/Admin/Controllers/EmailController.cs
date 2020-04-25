using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            var select = await _context.Emails.Where(e => e.Readed == false).ToListAsync();

            foreach (var item in select)
            {
                item.Readed = true;

                _context.Emails.Update(item);
            }

            await _context.SaveChangesAsync();
           

            return View(await _context.Emails.ToListAsync());
        }
    }
}