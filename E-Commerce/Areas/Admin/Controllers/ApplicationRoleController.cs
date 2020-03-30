using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = "Admin")]
	public class ApplicationRoleController : Controller
	{
		private readonly RoleManager<ApplicationRole> _roleManager;

		public ApplicationRoleController(RoleManager<ApplicationRole> roleManager)
		{
			_roleManager = roleManager;
		}

		[AutoValidateAntiforgeryToken]
		public IActionResult Index()
		{
			return View(_roleManager.Roles.ToList());
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddEdit(string id)
		{
			var applicationRole = await _roleManager.FindByIdAsync(id);

			if (applicationRole != null)
			{
				return PartialView("AddEdit", applicationRole);
			}

			return PartialView("AddEdit", new ApplicationRole { Id = String.Empty });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEdit(string id, ApplicationRole model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				var applicationRole = await _roleManager.FindByIdAsync(id) ?? new ApplicationRole();

				applicationRole.Name = model.Name;
				applicationRole.Description = model.Description;

				var role = !String.IsNullOrWhiteSpace(id) ? await _roleManager.UpdateAsync(applicationRole) :
					await _roleManager.CreateAsync(applicationRole);

				if (role.Succeeded)
				{
					return PartialView("_SuccessfulResponse", redirectUrl);
				}
			}
			return PartialView("AddEdit", model);
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role != null)
			{
				return PartialView("Delete", role.Name);
			}

			return PartialView("Delete", String.Empty);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, IFormCollection form)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role != null)
			{
				await _roleManager.DeleteAsync(role);
			}

			return View("Index");
		}
	}
}