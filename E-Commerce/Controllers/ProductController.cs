using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

				// related products
				ViewBag.RelatedProducts = await _context.Products.Where(x => x.CategoryId == product.CategoryId && x.CarId == product.CarId && x.Id != product.Id).ToListAsync();

				ViewBag.ProductFields = await _context.ProductFields.Where(x => x.ProductId == product.Id)
					.Include(x => x.Field).ToListAsync();
			}

			return View(product);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> CategoryGroup(string id)
		{
			return View(await _context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == id));
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Category(string id)
		{
			return View(await _context.Categories.FirstOrDefaultAsync(x => x.Id == id));
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Brand(string id)
		{
			return View(await _context.Brands.FirstOrDefaultAsync(x => x.Id == id));
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Search(string id)
		{
			return View(await _context.Products.FirstOrDefaultAsync(x => x.Id == id));
		}
	}
}