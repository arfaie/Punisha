using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
	public class ProductPropertiesViewComponent : ViewComponent
	{
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        public ProductPropertiesViewComponent(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;

            _env = env;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
		{
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            var selectField = _context.ProductFields.Where(x => x.ProductId == id).Include(x => x.Field)
                .Include(x => x.Field.FieldType).Include(x=>x.Product)
                .Include(x => x.Field.FieldGroup).Include(x => x.Field.CategoryFields);
            var filterCategory = selectField.Where(x => x.Field.CategoryFields.Where(y => y.CategoryId == product.CategoryId).Count() > 0);
            return View(filterCategory);
        }
	}
}