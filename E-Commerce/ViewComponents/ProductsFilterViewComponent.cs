using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.ViewComponents
{
	public class ProductsFilterViewComponent : ViewComponent
	{
		public ProductsFilterViewComponent()
		{
		}

		public async Task<IViewComponentResult> InvokeAsync(List<CategoryGroup> categoryGroups, List<Category> categories, List<Brand> brands, List<Car> cars, string makerId = "0", string carId = "0", string categoryGroupId = "0", string categoryId = "0", string brandId = "0")
		{
			ViewBag.CategoryGroups = categoryGroups;
			ViewBag.Categories = categories;

			ViewBag.Brands = brands;
			ViewBag.Cars = cars;

			ViewBag.MakerId = makerId;
			ViewBag.CarId = carId;
			ViewBag.CategoryGroupId = categoryGroupId;
			ViewBag.CategoryId = categoryId;
			ViewBag.BrandId = brandId;

			return View();
		}
	}
}