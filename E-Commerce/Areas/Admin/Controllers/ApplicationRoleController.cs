using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ApplicationRoleController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

		public ApplicationRoleController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public IActionResult Index()
		{
			List<ApplicationRole> model = new List<ApplicationRole>();
			model = _roleManager.Roles.Select(r => new ApplicationRole
			{
				Id = r.Id,
				Name = r.Name,
				Description = r.Description
			}).ToList();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddEditApplicationRole(string id)
		{
			ApplicationRole model = new ApplicationRole();
			if (!string.IsNullOrEmpty(id))
			{
				ApplicationRole applicationRole = await _roleManager.FindByIdAsync(id);

				if (applicationRole != null)
				{
					model.Id = applicationRole.Id;
					model.Name = applicationRole.Name;
					model.Description = applicationRole.Description;
				}
				else
				{
					return RedirectToAction("Index");
				}
			}

			return PartialView("AddEditApplicationRole", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddEditApplicationRole(string id, ApplicationRole model, string redirectUrl)
		{
			if (ModelState.IsValid)
			{
				bool isExist = !String.IsNullOrEmpty(id);
				ApplicationRole applicationRole = isExist ? await _roleManager.FindByIdAsync(id) :
					new ApplicationRole //insert
					{
					};

				applicationRole.Name = model.Name;
				applicationRole.Description = model.Description;

				IdentityResult roleresult = isExist ? await _roleManager.UpdateAsync(applicationRole) :
					await _roleManager.CreateAsync(applicationRole);

				if (roleresult.Succeeded)
				{
					return PartialView("_SuccessfulResponse", redirectUrl);
				}
			}
			return PartialView("AddEditApplicationRole", model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteRole(string id)
		{
			string name = string.Empty;
			if (!String.IsNullOrEmpty(id))
			{
				ApplicationRole ar = await _roleManager.FindByIdAsync(id);
				if (ar != null)
				{
					name = ar.Name;
				}
			}
			return PartialView("DeleteRole", name);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteRole(string id, IFormCollection form)
		{
			if (!string.IsNullOrEmpty(id))
			{
				ApplicationRole ar = await _roleManager.FindByIdAsync(id);
				if (ar != null)
				{
					IdentityResult roleresult = _roleManager.DeleteAsync(ar).Result;
					if (roleresult.Succeeded)
					{
						return RedirectToAction("Index");
					}
				}
			}
			return View("Index");
		}
	}
}