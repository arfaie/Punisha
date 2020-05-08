﻿using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

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

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DetailesBlog(string id)
        {
            ViewBag.path = "/upload/normalimage/";
            
            ViewBag.tags = await _context.Tags.ToListAsync();
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("BlogId")))
            {
                HttpContext.Session.SetString("BlogId", id);
            }
            
            return View(await _context.Newses.Include(n => n.NewCategories).Include(x => x.NewsTagses)
                .ThenInclude(x => x.tags).FirstOrDefaultAsync(x => x.Id == id));
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> searchBlog(string Titel, string categoryId, string tagId)
        {
            ViewBag.path = "/upload/normalimage/";

            if (!string.IsNullOrWhiteSpace(Titel))
            {
                return View("Index", await _context.Newses.Include(n => n.NewCategories).Include(x => x.NewsTagses)
                    .ThenInclude(x => x.tags).Where(x => x.Title.Contains(Titel)).ToListAsync());
            }

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                return View("Index", await _context.Newses.Include(n => n.NewCategories).Include(x => x.NewsTagses)
                    .ThenInclude(x => x.tags).Where(x => x.IdCategories == categoryId).ToListAsync());
            }

            if (!string.IsNullOrWhiteSpace(tagId))
            {
                var selectNewses = await _context.Newses.Include(n => n.NewCategories)
                    .Include(x => x.NewsTagses)
                    .ThenInclude(x => x.tags).ToListAsync();
                var selectTag = selectNewses.Where(x=>x.NewsTagses.Where(x => x.IdTag == tagId).Any()).ToList();
                return View("Index", selectTag);
            }

            return RedirectToAction("Index");
        }
    }
}