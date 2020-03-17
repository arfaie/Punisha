using System;
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
    public class CityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public CityController(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            //var model = await _context.Cities.ToListAsync();
            var model = await (from c in _context.Cities
                               join s in _context.States on c.StatesId equals s.Id
                               select new CityViewModel
                               {
                                   Id = c.Id,
                                   Name = c.Name,
                                   StatesId = c.StatesId,
                                   StateName = s.Name
                               }).ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditCity(int Id)
        {
            AddEditCityViewModel model = new AddEditCityViewModel();
            model.StaSelectListItems = await _context.States.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToListAsync();

            if (Id != 0)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    City city = await _context.Cities.Where(c => c.Id == Id).SingleOrDefaultAsync();
                    if (city != null)
                    {
                        model.Id = city.Id;
                        model.Name = city.Name;
                        model.StatesId = city.StatesId;
                    }
                }
            }

            return PartialView("AddEditCity", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditCity(int Id, AddEditCityViewModel model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                if (Id == 0)
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        City city = AutoMapper.Mapper.Map<AddEditCityViewModel, City>(model);
                        db.Cities.Add(city);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirecturl);
                }
                else
                {
                    using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        City city = AutoMapper.Mapper.Map<AddEditCityViewModel, City>(model);
                        db.Cities.Update(city);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);

                    return PartialView("_Succefullyresponse", redirecturl);
                }
            }
            else
            {
                if (Id == 0)
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.addError, type: ToastType.yellow);
                }
                else
                {
                    TempData["Notif"] = Notification.ShowNotif(MessageType.editError, type: ToastType.yellow);
                }

                model.StaSelectListItems = await _context.States.Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                }).ToListAsync();

                return PartialView("AddEditCity", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCity(int Id)
        {
            var city = new City();
            if (Id != 0)
            {
                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    city = await _context.Cities.Where(c => c.Id == Id).SingleOrDefaultAsync();
                    if (city == null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return PartialView("DeleteCity", city.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCity(int id, string redirecturl)
        {
            if (ModelState.IsValid)
            {

                using (var db = _serviceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var model = await db.Cities.Where(c => c.Id == id).SingleOrDefaultAsync();
                    db.Cities.Remove(model);
                    await db.SaveChangesAsync();
                }
                TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);

                return PartialView("_Succefullyresponse", redirecturl);
            }
            else
            {
                TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.yellow);

                return RedirectToAction("Index");
            }

        }
    }
}