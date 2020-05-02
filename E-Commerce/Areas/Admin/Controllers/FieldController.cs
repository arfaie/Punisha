using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Helpers;
using ECommerce.Models.Helpers.OptionEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// TODO field
namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FieldController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FieldController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            var select = await _context.Fields.ToListAsync();
            return View(select);
        }


        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
            ViewBag.FieldGroups = new SelectList(await _context.FieldGroups.ToListAsync(), "Id", "Title");
            ViewBag.FieldTypes = new SelectList(await _context.FieldTypes.ToListAsync(), "Id", "Title");
            if (id != null)
            {
                var fields = await _context.Fields.FirstOrDefaultAsync(c => c.Id == id);

                var select = await _context.CategoryFields.Where(x => x.FieldId == id).ToListAsync();
                fields.CategoryIds = select.Select(x => x.CategoryId).ToArray();
                return PartialView("AddEdit", fields);
            }
            return PartialView("AddEdit", new Field());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, Field model, string redirectUrl)
        {
            redirectUrl = "/Admin/Field";
            if (String.IsNullOrWhiteSpace(id))
            {
                if (ModelState.IsValid)
                {
                    var selectOrder = _context.Fields;
                    //var aa=selectOrder.Result;
                    int order = 0;
                    if (selectOrder.Count() != 0)
                    {
                        order = selectOrder.First().Order + 1;
                    }
                    model.Order = order;
                    await _context.Fields.AddAsync(model);
                    await _context.SaveChangesAsync();
                    try
                    {
                        var select1 = _context.Fields;
                    }
                    catch (System.Exception e)
                    {

                        throw;
                    }
                    var select = _context.Fields;
                    string ss = select.ToList().OrderByDescending(x => x.Order).First().Id;

                    foreach (var item in model.CategoryIds)
                    {
                        CategoryField CategoryField = new CategoryField();
                        CategoryField.FieldId = ss;
                        CategoryField.CategoryId = item;
                        await _context.CategoryFields.AddAsync(CategoryField);

                        //Add to ProductFields
                        ProductField pf = new ProductField();
                        var selectProducts = _context.Products.Where(x => x.CategoryId == item);
                        foreach (var itemProduct in selectProducts)
                        {
                            pf.ProductId = itemProduct.Id;
                            pf.FieldId = ss;
                            pf.Value = null;
                            await _context.ProductFields.AddAsync(pf);
                        }
                        //Add to ProductFields
                    }

                    await _context.SaveChangesAsync();
                    return PartialView("_SuccessfulResponse", redirectUrl);
                }
            }
            else
            {
                if (model.CategoryIds != null)
                {
                    var CategoryFields = await _context.CategoryFields.Where(x => x.FieldId == model.Id).ToListAsync();
                    foreach (var item in model.CategoryIds)
                    {
                        var isExist = CategoryFields.Where(x => x.CategoryId == item).FirstOrDefault();
                        if (isExist == null)
                        {
                            CategoryField CategoryField = new CategoryField();
                            CategoryField.FieldId = model.Id;
                            CategoryField.CategoryId = item;
                            await _context.CategoryFields.AddAsync(CategoryField);

                            //Add to ProductFields
                            ProductField pf = new ProductField();
                            var selectProducts = _context.Products.Where(x => x.CategoryId == item);
                            foreach (var itemProduct in selectProducts)
                            {
                                pf.ProductId = itemProduct.Id;
                                pf.FieldId = model.Id;
                                pf.Value = null;
                                await _context.ProductFields.AddAsync(pf);
                            }
                            //Add to ProductFields
                        }
                    }
                    foreach (var item in CategoryFields)
                    {
                        bool isExist = model.CategoryIds.Contains(item.CategoryId);
                        if (!isExist)
                        {
                            var s = _context.CategoryFields.Where(x => x.CategoryId == item.CategoryId).FirstOrDefault();
                            var ss =  _context.ProductFields.Where(x => x.FieldId == item.FieldId).FirstOrDefault();
                            _context.CategoryFields.Remove(s);
                            _context.ProductFields.Remove(ss);
                        }
                    }
                }

                //_context.CategoryFields.RemoveRange(CategoryFields);
                //_context.ProductFields.RemoveRange(await _context.ProductFields.Where(x => x.FieldId == model.Id).ToListAsync());

                //if (model.CategoryIds != null)
                //{
                //    foreach (var item in model.CategoryIds)
                //    {
                //        CategoryField CategoryField = new CategoryField();
                //        CategoryField.FieldId = model.Id;
                //        CategoryField.CategoryId = item;
                //        _context.CategoryFields.Add(CategoryField);

                //        //Add to ProductFields
                //        ProductField pf = new ProductField();
                //        var selectProducts = _context.Products.Where(x => x.CategoryId == item);
                //        foreach (var itemProduct in selectProducts)
                //        {
                //            pf.ProductId = itemProduct.Id;
                //            pf.FieldId = model.Id;
                //            pf.Value = null;
                //            _context.ProductFields.Add(pf);
                //        }
                //        //Add to ProductFields
                //    }
                //}
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return PartialView("_SuccessfulResponse", redirectUrl);
            }


            ViewBag.SelectGroups = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");
            ViewBag.FieldGroups = new SelectList(await _context.FieldGroups.ToListAsync(), "Id", "Title");
            ViewBag.FieldTypes = new SelectList(await _context.FieldTypes.ToListAsync(), "Id", "Title");

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var field = new Field();

            {
                field = await _context.Fields.SingleOrDefaultAsync(a => a.Id == id);
                if (field == null)
                {
                    return RedirectToAction("Index");
                }
            }

            return PartialView("Delete", field.Title);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                {
                    var selectCatField = _context.CategoryFields.Where(x => x.FieldId == id);
                    if (selectCatField.Count() != 0)
                    {
                        _context.CategoryFields.RemoveRange(selectCatField);
                    }
                    var selectProductField = _context.ProductFields.Where(x => x.FieldId == id);
                    if (selectProductField.Count() != 0)
                    {
                        _context.ProductFields.RemoveRange(selectProductField);
                    }
                    var field = await _context.Fields.SingleOrDefaultAsync(a => a.Id == id);

                    _context.Fields.Remove(field);
                    await _context.SaveChangesAsync();

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
                    return PartialView("_SuccessfulResponse", redirectUrl);
                }
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Yellow);

            return RedirectToAction("Index");
        }
    }
}