using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrderController : Controller
	{
		private readonly ApplicationDbContext _context;

		public OrderController(ApplicationDbContext context)
		{
			_context = context;
			//StiLicense.LoadFromString("6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHl2AD0gPVknKsaW0un+3PuM6TTcPMUAWEURKXNso0e5OJN40hxJjK5JbrxU+NrJ3E0OUAve6MDSIxK3504G4vSTqZezogz9ehm+xS8zUyh3tFhCWSvIoPFEEuqZTyO744uk+ezyGDj7C5jJQQjndNuSYeM+UdsAZVREEuyNFHLm7gD9OuR2dWjf8ldIO6Goh3h52+uMZxbUNal/0uomgpx5NklQZwVfjTBOg0xKBLJqZTDKbdtUrnFeTZLQXPhrQA5D+hCvqsj+DE0n6uAvCB2kNOvqlDealr9mE3y978bJuoq1l4UNE3EzDk+UqlPo8KwL1XM+o1oxqZAZWsRmNv4Rr2EXqg/RNUQId47/4JO0ymIF5V4UMeQcPXs9DicCBJO2qz1Y+MIpmMDbSETtJWksDF5ns6+B0R7BsNPX+rw8nvVtKI1OTJ2GmcYBeRkIyCB7f8VefTSOkq5ZeZkI8loPcLsR4fC4TXjJu2loGgy4avJVXk32bt4FFp9ikWocI9OQ7CakMKyAF6Zx7dJF1nZw");
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.Orders.Include(o => o.Factor).Include(o => o.Status).OrderByDescending(x => x.TransactionDate).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> addIssueCode(string id)
		{
			ViewBag.Status = new SelectList(await _context.Statuses.ToListAsync(), "Id", "Title");

			ViewBag.Factor = new SelectList(await _context.Factors.ToListAsync(), "Id", "FactorCode");

			var order = await _context.Orders.Where(o => o.Id == id).SingleOrDefaultAsync();
			if (order != null)
			{
				return PartialView("ChangeStatus", order);
			}

			return null;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> addIssueCode(string id, Order model, string redirectUrl, long intIssueCode)
		{
			if (ModelState.IsValid)
			{
				if (!string.IsNullOrEmpty(id))
				{
					if (intIssueCode != null)
					{
						model.IssueCode = intIssueCode;
					}

					_context.Orders.Update(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				return null;
			}

			return PartialView("ChangeStatus", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> orderFactor(string id)
		{
			var Select = await _context.Orders.Include(x => x.Factor).ThenInclude(x => x.FactorItems)
				.ThenInclude(x => x.Product).ThenInclude(x => x.Unit).Include(x => x.Factor).ThenInclude(x => x.Address)
				.FirstOrDefaultAsync(x => x.Id == id);
			return View(Select);
		}

		public async Task<IActionResult> Print(string id)
		{
			StiReport report = new StiReport();

			report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/ReportFactor.mrt"));

			var Select = await _context.Orders.Include(x => x.Factor).ThenInclude(x => x.FactorItems)
				.ThenInclude(x => x.Product).ThenInclude(x => x.Unit).Include(x => x.Factor).ThenInclude(x => x.Address)
				.FirstOrDefaultAsync(x => x.Id == id);

			report.RegData("dtFactor", Select);

			return StiNetCoreViewer.GetReportResult(this, report);
		}

		public IActionResult ViewerEvent()
		{
			return StiNetCoreViewer.ViewerEventResult(this);
		}
	}
}