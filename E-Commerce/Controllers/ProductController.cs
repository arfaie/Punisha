using ECommerce.Data;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

				var carProductsIds = await _context.CarProducts.Where(x => x.ProductId == product.Id).Select(x => x.CarId).ToListAsync();

				ViewBag.Cars = await _context.Cars.Where(x => carProductsIds.Contains(x.Id)).Include(x => x.Maker).ToListAsync();

				ViewBag.RelatedProducts = await _context.Products.Where(x => x.Id != product.Id && x.CategoryId == product.CategoryId && x.CarProducts.Any(y => carProductsIds.Contains(y.CarId))).ToListAsync();

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

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();

			return View(await _context.Products.Where(x => x.Category.CategoryGroupId == id).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Category(string id)
		{
			var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

			if (category != null)
			{
				ViewBag.Category = category;
				ViewBag.CategoryGroup = await _context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == category.CategoryGroupId);
			}

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();

			return View(await _context.Products.Where(x => x.CategoryId == id).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Brand(string id)
		{
			ViewBag.Brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();

			return View(await _context.Products.Where(x => x.BrandId == id).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Car(string id)
		{
			var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

			if (car != null)
			{
				ViewBag.Car = car;
				ViewBag.Maker = await _context.Makers.FirstOrDefaultAsync(x => x.Id == car.MakerId);
			}

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();

			return View(await _context.Products.Where(x => x.CarProducts.Select(y => y.CarId).Contains(id)).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Maker(string id)
		{
			ViewBag.Maker = await _context.Makers.FirstOrDefaultAsync(x => x.Id == id);

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();

			return View(await _context.Products.Where(x => x.CarProducts.Select(y => y.Car.Maker.Id).Contains(id)).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Search(string id)
		{
			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();

			return View(await _context.Products.Where(x => x.Name.Contains(id)).Include(x => x.Category).Include(x => x.Brand).ToListAsync());
		}

		[HttpPost]
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

		[HttpPost]
		public async Task<IActionResult> ChangeFactorItemCount(string factorItemId, int value)
		{
			var factorItem = await _context.FactorItems.FirstOrDefaultAsync(x => x.Id == factorItemId);

			if (factorItem != null)
			{
				var changeValue = value - factorItem.UnitCount;
				factorItem.UnitCount = value;

				_context.FactorItems.Update(factorItem);
				await _context.SaveChangesAsync();

				return Json(new { status = "success", changeValue, unitPrice = factorItem.UnitPrice });
			}
			return Json(new { status = "success", changeValue = 0, unitPrice = 0 });
		}

		[HttpPost]
		public async Task<IActionResult> RemoveFactorItem(string factorItemId)
		{
			var factorItem = await _context.FactorItems.FirstOrDefaultAsync(x => x.Id == factorItemId);

			if (factorItem != null)
			{
				if (HttpContext.Session.Keys.Contains("CartItems"))
				{
					var cartItems = HttpContext.Session.GetComplexData<List<string>>("CartItems");

					var list = new List<string>(cartItems);
					list.Remove(factorItem.ProductId);

					HttpContext.Session.SetComplexData("CartItems", list.ToArray());
				}

				var value = factorItem.UnitCount;

				_context.FactorItems.Remove(factorItem);
				await _context.SaveChangesAsync();

				return Json(new { status = "success", value });
			}
			return Json(new { status = "success", value = 0 });
		}
	}
}