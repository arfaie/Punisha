using ECommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AddressController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AddressController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _context.Addresses.ToListAsync();
			//				   join u in _context.Users on address.UserId equals u.Id
			//				   join c in _context.Cities on address.CityId equals c.Id
			//				   join s in _context.States on c.StateId equals s.Id
			//				   select new Address
			//				   {
			//					   Id = address.Id,
			//					   StateName = s.Name,
			//					   CityId = address.CityId,
			//					   CityName = c.Name,
			//					   Address = address.Address,
			//					   Plaque = address.Plaque,
			//					   PostalCode = address.PostalCode,
			//					   Mobile = address.Mobile,
			//					   Phone = address.Phone,
			//					   Lan = address.Lan,
			//					   Lat = address.Lat,
			//					   UserId = u.Id,
			//					   UserFullName = u.FullName + " " + u.Lastname
			//				   })

			return View(model);
		}
	}
}