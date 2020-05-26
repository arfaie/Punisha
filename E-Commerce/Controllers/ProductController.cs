using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<ApplicationUser> _userManager;

		public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Details(string id)
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			var product = await _context.Products.Include(x => x.ProductGalleries).Include(x => x.CommentAndStars).ThenInclude(x => x.User).Include(x => x.Brand).Include(x => x.OfferItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);

			if (product != null)
			{
				if (user != null)
				{
					var history = await _context.Histories.FirstOrDefaultAsync(x => x.ProductId == product.Id && x.UserId == user.Id);

					if (history != null)
					{
						history.RegistrationDateAndTime = DateTime.UtcNow;
						_context.Histories.Update(history);
					}
					else
					{
						history = new History
						{
							ProductId = product.Id,
							UserId = user.Id,
							RegistrationDateAndTime = DateTime.UtcNow
						};
						await _context.Histories.AddAsync(history);
					}
					await _context.SaveChangesAsync();
				}

				await Helper.AddOfferToProductAsync(_context, user, product);

				// breadcrumb data
				var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == product.CategoryId);

				if (category != null)
				{
					ViewBag.Category = category;

					ViewBag.CategoryGroup = await _context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == category.CategoryGroupId);
				}

				var carProductsIds = await _context.CarProducts.Where(x => x.ProductId == product.Id).Select(x => x.CarId).ToListAsync();

				ViewBag.Cars = await _context.Cars.Where(x => carProductsIds.Contains(x.Id)).Include(x => x.Maker).ToListAsync();

				var products = _context.Products.Where(x => x.Id != product.Id && x.CategoryId == product.CategoryId && x.CarProducts.Any(y => carProductsIds.Contains(y.CarId))).Include(x => x.OfferItems).ToList();

				await Helper.AddOfferToProductsAsync(_context, user, products);

				ViewBag.RelatedProducts = products;

				ViewBag.ProductFields = await _context.ProductFields.Where(x => x.ProductId == product.Id)
					.Include(x => x.Field).ToListAsync();
			}

			return View(product);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> CategoryGroup(string id, int skip = 0, int limit = 12)
		{
			ViewBag.CategoryGroupId = id;
			ViewBag.CategoryGroup = await _context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == id);

			var products = await _context.Products.Where(x => x.Category.CategoryGroupId == id).Include(x => x.Category).Include(x => x.Brand).Include(x => x.FactorItems).Include(x => x.OfferItems).ToListAsync();

			ViewBag.Count = products.Count;

			if (products.Count > 0)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
				await Helper.AddOfferToProductsAsync(_context, user, products);

				ViewBag.MinPrice = products.Min(x => x.PriceWithDiscount);
				ViewBag.MaxPrice = products.Max(x => x.Price);

				products = products.Skip(skip * limit).Take(limit).ToList();
			}

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Cars = await _context.Cars.Include(x => x.Maker).OrderBy(x => x.Name).ToListAsync();
			ViewBag.Skip = skip;
			ViewBag.Limit = limit;

			return View(products);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Category(string id, int skip = 0, int limit = 12)
		{
			ViewBag.CategoryId = id;
			var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

			if (category != null)
			{
				ViewBag.Category = category;
				ViewBag.CategoryGroup = await _context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == category.CategoryGroupId);
			}

			var products = await _context.Products.Where(x => x.CategoryId == id).Include(x => x.Category).Include(x => x.Brand).Include(x => x.FactorItems).Include(x => x.OfferItems).ToListAsync();

			ViewBag.Count = products.Count;

			if (products.Count > 0)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
				await Helper.AddOfferToProductsAsync(_context, user, products);

				ViewBag.MinPrice = products.Min(x => x.PriceWithDiscount);
				ViewBag.MaxPrice = products.Max(x => x.Price);

				products = products.Skip(skip * limit).Take(limit).ToList();
			}

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Cars = await _context.Cars.Include(x => x.Maker).OrderBy(x => x.Name).ToListAsync();
			ViewBag.Skip = skip;
			ViewBag.Limit = limit;

			return View(products);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Brand(string id, int skip = 0, int limit = 12)
		{
			ViewBag.BrandId = id;
			ViewBag.Brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

			var products = await _context.Products.Where(x => x.BrandId == id).Include(x => x.Category).Include(x => x.Brand).Include(x => x.OfferItems).ToListAsync();

			ViewBag.Count = products.Count;

			if (products.Count > 0)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
				await Helper.AddOfferToProductsAsync(_context, user, products);

				ViewBag.MinPrice = products.Min(x => x.PriceWithDiscount);
				ViewBag.MaxPrice = products.Max(x => x.Price);

				products = products.Skip(skip * limit).Take(limit).ToList();
			}

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Cars = await _context.Cars.Include(x => x.Maker).OrderBy(x => x.Name).ToListAsync();
			ViewBag.Skip = skip;
			ViewBag.Limit = limit;

			return View(products);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Maker(string id, int skip = 0, int limit = 12)
		{
			ViewBag.MakerId = id;
			ViewBag.Maker = await _context.Makers.FirstOrDefaultAsync(x => x.Id == id);
			var products = await _context.Products.Where(x => x.CarProducts != null && x.CarProducts.Select(y => y.Car.Maker.Id).Contains(id)).Include(x => x.Category).Include(x => x.Brand).Include(x => x.FactorItems).Include(x => x.OfferItems).ToListAsync();

			ViewBag.Count = products.Count;

			if (products.Count > 0)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
				await Helper.AddOfferToProductsAsync(_context, user, products);

				ViewBag.MinPrice = products.Min(x => x.PriceWithDiscount);
				ViewBag.MaxPrice = products.Max(x => x.Price);

				products = products.Skip(skip * limit).Take(limit).ToList();
			}

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Cars = await _context.Cars.Include(x => x.Maker).OrderBy(x => x.Name).ToListAsync();
			ViewBag.Skip = skip;
			ViewBag.Limit = limit;

			return View(products);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Car(string id, int skip = 0, int limit = 12)
		{
			ViewBag.CarId = id;
			var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

			if (car != null)
			{
				ViewBag.Car = car;
				ViewBag.Maker = await _context.Makers.FirstOrDefaultAsync(x => x.Id == car.MakerId);
			}

			var products = await _context.Products.Where(x => x.CarProducts != null && x.CarProducts.Select(y => y.CarId).Contains(id)).Include(x => x.Category).Include(x => x.Brand).Include(x => x.FactorItems).Include(x => x.OfferItems).ToListAsync();

			ViewBag.Count = products.Count;

			if (products.Count > 0)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
				await Helper.AddOfferToProductsAsync(_context, user, products);

				ViewBag.MinPrice = products.Min(x => x.PriceWithDiscount);
				ViewBag.MaxPrice = products.Max(x => x.Price);

				products = products.Skip(skip * limit).Take(limit).ToList();
			}

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Cars = await _context.Cars.Include(x => x.Maker).OrderBy(x => x.Name).ToListAsync();
			ViewBag.Skip = skip;
			ViewBag.Limit = limit;

			return View(products);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Search(string makerId = "0", string carId = "0", string categoryGroupId = "0", string categoryId = "0", string brandId = "0", int minPrice = 0, int maxPrice = 0, string categories = "", string brands = "", string cars = "", int sorting = 0, int skip = 0, int limit = 12, bool showOnlyInStock = false, bool isAjax = false)
		{
			var products = await _context.Products.Include(x => x.Category).Include(x => x.Brand).Include(x => x.FactorItems).Include(x => x.OfferItems).Include(x => x.CarProducts).ThenInclude(x => x.Car).ToListAsync();

			if (showOnlyInStock)
			{
				products = products.Where(x => x.Inventory > x.OrderPoint).ToList();
			}

			if (!String.IsNullOrWhiteSpace(carId) && carId != "0")
			{
				products = products.Where(x => x.CarProducts != null && x.CarProducts.Select(y => y.CarId).Contains(carId)).ToList();
			}
			else if (!String.IsNullOrWhiteSpace(makerId) && makerId != "0")
			{
				products = products.Where(x => x.CarProducts != null && x.CarProducts.Select(y => y.Car.MakerId).Contains(makerId)).ToList();
			}

			if (!String.IsNullOrWhiteSpace(categoryGroupId) && categoryGroupId != "0")
			{
				products = products.Where(x => x.Category.CategoryGroupId == categoryGroupId).ToList();
			}
			else if (!String.IsNullOrWhiteSpace(categoryId) && categoryId != "0")
			{
				products = products.Where(x => x.CategoryId == categoryId).ToList();
			}

			if (!String.IsNullOrWhiteSpace(brandId) && brandId != "0")
			{
				products = products.Where(x => x.BrandId == brandId).ToList();
			}

			//var enumerable = products.ToList();
			var minimumPrice = 0;
			var maximumPrice = 0;

			if (products.Count > 0)
			{
				var user = await _userManager.GetUserAsync(HttpContext.User);
				await Helper.AddOfferToProductsAsync(_context, user, products);

				minimumPrice = (int)products.Min(x => x.PriceWithDiscount);
				maximumPrice = products.Max(x => x.Price);

				products = applyFilterAndSort(products, minPrice, maxPrice, categories, brands, cars, sorting);
			}

			ViewBag.Count = products.Count;

			if (products.Count > 0)
			{
				products = products.Skip(skip * limit).Take(limit).ToList();
			}

			if (isAjax)
			{
				foreach (var product in products)
				{
					product.CarIds = null;
					product.CarProducts = null;
					product.Category = null;
					product.CommentAndStars = null;
					product.FactorItems = null;
					product.Histories = null;
					product.OfferItems = null;
					product.ProductFields = null;
					product.ProductGalleries = null;
					product.Unit = null;

					if (product.Brand != null)
					{
						product.Brand.Products = null;
					}
				}

				var result = new { products, count = ViewBag.Count }; //, minPrice = minimumPrice, maxPrice = maximumPrice
				return Json(result);
			}

			ViewBag.MinPrice = minimumPrice;
			ViewBag.MaxPrice = maximumPrice;

			ViewBag.CategoryGroups = await _context.CategoryGroups.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Categories = await _context.Categories.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Brands = await _context.Brands.OrderBy(x => x.Title).ToListAsync();
			ViewBag.Cars = await _context.Cars.Include(x => x.Maker).OrderBy(x => x.Name).ToListAsync();
			ViewBag.Skip = skip;
			ViewBag.Limit = limit;

			ViewBag.MakerId = makerId;
			ViewBag.CarId = carId;
			ViewBag.CategoryGroupId = categoryGroupId;
			ViewBag.CategoryId = categoryId;
			ViewBag.brandId = brandId;

			return View(products);
		}

		private static List<Product> applyFilterAndSort(List<Product> products, int minPrice, int maxPrice, string categories, string brands, string cars, int sorting)
		{
			if (!String.IsNullOrWhiteSpace(categories))
			{
				products = products.Where(x => categories.Contains(x.CategoryId)).ToList();
			}

			if (!String.IsNullOrWhiteSpace(brands))
			{
				products = products.Where(x => brands.Contains(x.BrandId)).ToList();
			}

			if (!String.IsNullOrWhiteSpace(cars))
			{
				products = products.Where(x => x.CarProducts.Any(y => cars.Contains(y.CarId))).ToList();
			}

			if (minPrice != 0)
			{
				products = products.Where(x => x.PriceWithDiscount >= minPrice).ToList();
			}

			if (maxPrice != 0)
			{
				products = products.Where(x => x.Price <= maxPrice).ToList();
			}

			switch (sorting)
			{
				case 1: // پرفروش ترین
					products = products.OrderByDescending(x => x.FactorItems?.Count).ToList();
					break;

				case 2: // ارزان ترین
					products = products.OrderBy(x => x.Price).ToList();
					break;

				case 3: // گران ترین
					products = products.OrderByDescending(x => x.Price).ToList();
					break;

				default: // جدیدترین
					products = products.OrderByDescending(x => x.AddingDateTime).ToList();
					break;
			}

			return products;
		}

		[HttpGet]
		public async Task<IActionResult> SearchBox(string search)
		{
			var list = new List<Select2Model>();

			var products = await _context.Products.Where(x => x.Name.Contains(search)).ToListAsync();

			foreach (var product in products)
			{
				list.Add(new Select2Model { text = product.Name, id = product.Id });
			}

			var categoryGroups = await _context.CategoryGroups.Where(x => x.Title.Contains(search)).ToListAsync();

			foreach (var categoryGroup in categoryGroups)
			{
				list.Add(new Select2Model { text = $"جستجو در گروه {categoryGroup.Title}", id = $"!{categoryGroup.Id}" });
			}

			var categories = await _context.Categories.Where(x => x.Title.Contains(search)).ToListAsync();

			foreach (var category in categories)
			{
				list.Add(new Select2Model { text = $"جستجو در دسته بندی {category.Title}", id = $"@{category.Id}" });
			}

			var makers = await _context.Makers.Where(x => x.Name.Contains(search)).ToListAsync();

			foreach (var maker in makers)
			{
				list.Add(new Select2Model { text = $"جستجو در نوع خودروی {maker.Name}", id = $"#{maker.Id}" });
			}

			var cars = await _context.Cars.Where(x => x.Name.Contains(search)).ToListAsync();

			foreach (var car in cars)
			{
				list.Add(new Select2Model { text = $"جستجو در مدل خودروی {car.Name}", id = $"${car.Id}" });
			}

			var brands = await _context.Brands.Where(x => x.Title.Contains(search)).ToListAsync();

			foreach (var brand in brands)
			{
				list.Add(new Select2Model { text = $"جستجو در برند {brand.Title}", id = $"%{brand.Id}" });
			}

			return Json(new { items = list });
		}

		[HttpPost]
		public async Task<IActionResult> AddCommentAndStar(string productId, string stars, string comment)
		{
			var product = await _context.Products.SingleOrDefaultAsync(b => b.Id == productId);
			if (product == null)
			{
				return Json(new { status = "fail", message = "خطا در یافتن محصول" });
			}

			var user = await _userManager.GetUserAsync(HttpContext.User);

			if (user == null)
			{
				return Json(new { status = "fail", message = "خطا در یافتن کاربر" });
			}

			var feedback = await _context.CommentAndStars.FirstOrDefaultAsync(x => x.UserId == user.Id && x.ProductId == productId);

			if (feedback != null)
			{
				return Json(new { status = "fail", message = "نظر شما درباره این محصول قبلاً ثبت شده است" });
			}

			if (stars == "0" && String.IsNullOrWhiteSpace(comment))
			{
				return Json(new { status = "fail", message = "لطفاً نظر و امتیاز را وارد کنید." });
			}

			feedback = new CommentAndStar
			{
				Comment = comment,
				Stars = Convert.ToInt32(stars),
				Date = DateTime.UtcNow,
				ProductId = productId,
				UserId = user.Id
			};

			await _context.CommentAndStars.AddAsync(feedback);

			await _context.SaveChangesAsync();

			return Json(new { status = "success", message = "نظر شما ثبت شد. با تشکر" });
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

				return Json(new { status = "success", changeValue, unitPrice = factorItem.UnitPrice, discount = factorItem.Discount });
			}
			return Json(new { status = "success", changeValue = 0, unitPrice = 0, discount = 0 });
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