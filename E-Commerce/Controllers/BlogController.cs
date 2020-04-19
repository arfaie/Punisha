using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            ViewBag.path = "/upload/normalimage/";

            ViewBag.tags = await _context.Tags.ToListAsync();

            return View(await _context.Newses.Include(n => n.NewCategories).Include(x => x.NewsTagses).ThenInclude(x => x.tags).OrderByDescending(x => x.Id).ToListAsync());
        }
    }
}