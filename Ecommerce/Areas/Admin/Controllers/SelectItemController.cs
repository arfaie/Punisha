using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Helpers.OptionEnums;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Net;
using Ecommerce.Net.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SelectItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public SelectItemController(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var query = await (from si in _context.SelectItems
                join Sg in _context.SelectGroups on si.IdSelectGroup equals Sg.Id
                select new SelectItemViewModel
                {
                    Id = si.Id,
                    IdSelectGroup = si.IdSelectGroup,
                    SelectGroupTitle = Sg.Title,
                    Title = si.Title
                }).ToListAsync();

            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditSelectItem(int id)
        {

            AddEditSelectItemViewModel model = new AddEditSelectItemViewModel();
            model.SelectGroupListItems = await _context.SelectGroups.Select(c => new SelectListItem()
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            if (id != 0)
            {
                using (_serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    SelectItem SelectItem = await _context.SelectItems.Where(a => a.Id == id).SingleOrDefaultAsync();
                    if (SelectItem != null)
                    {
                        model.Id = SelectItem.Id;
                        model.Title = SelectItem.Title;
                        model.IdSelectGroup = SelectItem.IdSelectGroup;
                    }

                }
            }

            return PartialView("AddEditSelectItem", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditSelectItem(int Id, AddEditSelectItemViewModel model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (Id == 0)
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        SelectItem SelectItemModel = AutoMapper.Mapper.Map<AddEditSelectItemViewModel, SelectItem>(model);
                        db.SelectItems.Add(SelectItemModel);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
                else
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        SelectItem SelectItemModel = AutoMapper.Mapper.Map<AddEditSelectItemViewModel, SelectItem>(model);
                        db.SelectItems.Update(SelectItemModel);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
            }

            if (Id == 0)
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.addError, type: ToastType.yellow);
            }
            else
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
            }

            model.SelectGroupListItems = await _context.SelectGroups.Select(c => new SelectListItem()
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            return PartialView("AddEditSelectItem", model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSelectItem(int Id)
        {
            var SelectItem = new SelectItem();
            using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                SelectItem = await db.SelectItems.Where(a => a.Id == Id).SingleOrDefaultAsync();
                if (SelectItem == null)
                {
                    return RedirectToAction("Index");
                }
            }

            return PartialView("DeleteSelectItem", SelectItem.Title);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelectItem(int id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var SelectItem = await db.SelectItems.Where(a => a.Id == id).SingleOrDefaultAsync();

                    db.SelectItems.Remove(SelectItem);
                    await db.SaveChangesAsync();

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);

                    return PartialView("_Succefullyresponse", redirectUrl);
                }
            }

            TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.yellow);

            return RedirectToAction("Index");

        }
    }
}