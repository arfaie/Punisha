﻿using ECommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class HistoryController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HistoryController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index(string id)
		{
			return View(await _context.Histories.Where(h => h.UserId == id).Include(u => u.User)
				.Include(p => p.Product).ToListAsync());
		}
	}
}