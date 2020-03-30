using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CommentAndStarController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CommentAndStarController(ApplicationDbContext context)
		{
			_context = context;
		}

		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Index()
		{
			return View(await _context.CommentAndStars.Include(x => x.User).Include(x => x.Product).ToListAsync());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEdit(string id)
		{
			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");

			var commentAndStar = await _context.CommentAndStars.FirstOrDefaultAsync(c => c.Id == id);
			if (commentAndStar != null)
			{
				return PartialView("AddEdit", commentAndStar);
			}

			return PartialView("AddEdit", new CommentAndStar());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEdit(string id, CommentAndStar model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				if (String.IsNullOrWhiteSpace(id))
				{
					_context.CommentAndStars.Add(model);
					await _context.SaveChangesAsync();

					TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

					return PartialView("_SuccessfulResponse", redirectUrl);
				}

				_context.CommentAndStars.Update(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
			ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");

			return PartialView("AddEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var commentAndStar = await _context.CommentAndStars.Include(x => x.User).Include(x => x.Product).FirstOrDefaultAsync(c => c.Id == id);
			if (commentAndStar == null)
			{
				return RedirectToAction("Index");
			}

			return PartialView("Delete", $"{commentAndStar.Product?.Name} {commentAndStar.User?.UserName}");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var model = await _context.CommentAndStars.FirstOrDefaultAsync(c => c.Id == id);

				_context.CommentAndStars.Remove(model);
				await _context.SaveChangesAsync();

				TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

				return PartialView("_SuccessfulResponse", redirectUrl);
			}

			TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

			return RedirectToAction("Index");
		}
	}
}