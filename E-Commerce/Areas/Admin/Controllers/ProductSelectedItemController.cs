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
	public class ProductSelectedItemController : Controller
	{
		//private readonly ApplicationDbContext _context;

		//public ProductSelectedItemController(ApplicationDbContext context)
		//{
		//	_context = context;
		//}

		//[AutoValidateAntiforgeryToken]
		//public async Task<IActionResult> Index()
		//{
		//	return View(await _context.ProductSelectedItems.Include(x => x.ProductField).Include(x => x.SelectItem).ToListAsync());
		//}

		//[HttpGet]
		//[AutoValidateAntiforgeryToken]
		//public async Task<IActionResult> AddEdit(string id)
		//{
		//	ViewBag.ProductFields = new SelectList(await _context.ProductFields.ToListAsync(), "Id", "Value");
		//	ViewBag.SelectItem = new SelectList(await _context.SelectItems.ToListAsync(), "Id", "Title");

		//	var productSelectedItem = await _context.ProductSelectedItems.FirstOrDefaultAsync(c => c.Id == id);
		//	if (productSelectedItem != null)
		//	{
		//		return PartialView("AddEdit", productSelectedItem);
		//	}

		//	return PartialView("AddEdit", new ProductSelectedItem());
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> AddEdit(string id, ProductSelectedItem model, string redirectUrl)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		if (String.IsNullOrWhiteSpace(id))
		//		{
		//			_context.ProductSelectedItems.Add(model);
		//			await _context.SaveChangesAsync();

		//			TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

		//			return PartialView("_SuccessfulResponse", redirectUrl);
		//		}

		//		_context.ProductSelectedItems.Update(model);
		//		await _context.SaveChangesAsync();

		//		TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

		//		return PartialView("_SuccessfulResponse", redirectUrl);
		//	}

		//	ViewBag.ProductFields = new SelectList(await _context.ProductFields.ToListAsync(), "Id", "Value");
		//	ViewBag.SelectItem = new SelectList(await _context.SelectItems.ToListAsync(), "Id", "Title");

		//	return PartialView("AddEdit", model);
		//}

		//[HttpGet]
		//[AutoValidateAntiforgeryToken]
		//public async Task<IActionResult> Delete(string id)
		//{
		//	var productSelectedItem = await _context.ProductSelectedItems.Include(x => x.ProductField).Include(x => x.SelectItem).FirstOrDefaultAsync(c => c.Id == id);
		//	if (productSelectedItem == null)
		//	{
		//		return RedirectToAction("Index");
		//	}

		//	return PartialView("Delete", $"{productSelectedItem.SelectItem?.Title} {productSelectedItem.ProductField?.Value}");
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Delete(string id, string redirectUrl)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var model = await _context.ProductSelectedItems.FirstOrDefaultAsync(c => c.Id == id);

		//		_context.ProductSelectedItems.Remove(model);
		//		await _context.SaveChangesAsync();

		//		TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);

		//		return PartialView("_SuccessfulResponse", redirectUrl);
		//	}

		//	TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

		//	return RedirectToAction("Index");
		//}
	}
}