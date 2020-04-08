using ECommerce.Data;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ProductController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Details(string id)
		{
			var product = await _context.Products.Include(x => x.ProductGalleries).Include(x => x.CommentAndStars).Include(x => x.Brand).FirstOrDefaultAsync(x => x.Id == id);

			if (product != null)
			{
				// breadcrumb data
				var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == product.CategoryId);

				if (category != null)
				{
					ViewBag.Category = category;

					ViewBag.CategoryGroup = await _context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == category.CategoryGroupId);
				}

				ViewBag.RelatedProducts = await _context.Products.Where(x => x.CategoryId == product.CategoryId && x.CarIds == product.CarIds && x.Id != product.Id).ToListAsync();

				ViewBag.ProductFields = await _context.ProductFields.Where(x => x.ProductId == product.Id)
					.Include(x => x.Field).ToListAsync();
			}

			return View(product);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> CategoryGroup(string id)
		{
			ViewBag.CategoryGroup = await _context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == id);

			return View(await _context.Products.Where(x => x.Category.CategoryGroupId == id).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Category(string id)
		{
			ViewBag.Category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

			return View(await _context.Products.Where(x => x.CategoryId == id).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Brand(string id)
		{
			ViewBag.Brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

			return View(await _context.Products.Where(x => x.BrandId == id).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Search(string id)
		{
			return View(await _context.Products.Where(x => x.Name.Contains(id)).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		public async Task<IActionResult> AddToCart(string productId)
		{
			var product = await _context.Products.SingleOrDefaultAsync(b => b.Id == productId);
			if (product == null)
			{
				return Json(new { status = "fail", message = "این محصول موجود نیست" });
			}

			if (product.Inventory == 0)
			{
				return Json(new { status = "fail", message = "این محصول در انبار موجود نیست" });
			}

			if (!HttpContext.Session.Keys.Contains("CartItems"))
			{
				HttpContext.Session.SetComplexData("CartItems", new List<string> { productId });

				return Json(new { status = "success", message = "محصول به لیست درخواستی شما اضافه شد", count = 1 });
			}

			var cartItems = HttpContext.Session.GetComplexData<List<string>>("CartItems");

			if (cartItems.Contains(productId))
			{
				return Json(new { status = "success", message = "این محصول از قبل در لیست درخواستی شما وجود دارد", count = cartItems.Count });
			}

			cartItems.Add(productId);

			HttpContext.Session.SetComplexData("CartItems", cartItems);

			return Json(new { status = "success", message = "محصول به لیست درخواستی شما اضافه شد", count = cartItems.Count });
		}
	}
}