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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AgencyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iServiceProvider;

        public AgencyController(ApplicationDbContext context, IServiceProvider iServiceProvider)
        {
            _context = context;
            _iServiceProvider = iServiceProvider;
        }

        public async Task<IActionResult> Index()
        {
            var query = await (from a in _context.Agencies
                               join c in _context.Cities on a.IdCity equals c.Id
                              
                               select new AgencyViewModel()
                               {
                                   Id = a.Id,
                                   Title = a.Title,
                                   FirstName = a.FirstName,
                                   LastName = a.LastName,
                                  
                                  
                                   IdCity = a.IdCity,
                                   CityName = c.Name,
                                   Address = a.Address,
                                   Plaque = a.Plaque,
                                   PostalCode = a.PostalCode,
                                   Tell = a.Tell
                               }).ToListAsync();

            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditAgancy(int Id)
        {
            AddEditAgencyViewModel model = new AddEditAgencyViewModel();
            model.CitiesList = await _context.Cities.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();

            model.StatesList = await _context.States.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToListAsync();

            if (Id != 0)
            {
                using (_iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    Agency agency = await _context.Agencies.Where(a => a.Id == Id).SingleOrDefaultAsync();
                    City city =await  _context.Cities.Where(c => c.Id == agency.IdCity).SingleOrDefaultAsync();
                    if (agency != null)
                    {
                        model.Id = agency.Id;
                        model.Title = agency.Title;
                        model.FirstName = agency.FirstName;
                        model.LastName = agency.LastName;
                        model.IdState = city.StatesId;
                        model.IdCity = agency.IdCity;
                        model.Address = agency.Address;
                        model.Plaque = agency.Plaque;
                        model.PostalCode = agency.PostalCode;
                        model.Tell = agency.Tell;
                    }

                }
            }

            return PartialView("AddEditAgancy", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditAgancy(int Id, AddEditAgencyViewModel model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                if (Id == 0)
                {
                    using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        Agency agencymodel = AutoMapper.Mapper.Map<AddEditAgencyViewModel, Agency>(model);
                        db.Agencies.Add(agencymodel);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Add, type: ToastType.green);

                    return PartialView("_Succefullyresponse", redirecturl);
                }
                else
                {
                    using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        Agency agencymodel = AutoMapper.Mapper.Map<AddEditAgencyViewModel, Agency>(model);
                        db.Agencies.Update(agencymodel);
                        await db.SaveChangesAsync();
                    }

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Edit, type: ToastType.blue);

                    return PartialView("_Succefullyresponse", redirecturl);
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

            model.CitiesList = await _context.Cities.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();

            model.StatesList = await _context.States.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToListAsync();

            return PartialView("AddEditAgancy", model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAgency(int Id)
        {
            var agency = new Agency();
            using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                agency = await db.Agencies.Where(a => a.Id == Id).SingleOrDefaultAsync();
                if (agency == null)
                {
                    return RedirectToAction("Index");
                }
            }

            return PartialView("DeleteAgency", agency.Title);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAgency(int Id, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var agency = await db.Agencies.Where(a => a.Id == Id).SingleOrDefaultAsync();

                    db.Agencies.Remove(agency);
                    await db.SaveChangesAsync();

                    TempData["Notif"] = Notification.ShowNotif(MessageType.Delete, type: ToastType.red);

                    return PartialView("_Succefullyresponse", redirecturl);
                }
            }

            TempData["Notif"] = Notification.ShowNotif(MessageType.deleteError, type: ToastType.yellow);

            return RedirectToAction("Index");

        }
    }
}