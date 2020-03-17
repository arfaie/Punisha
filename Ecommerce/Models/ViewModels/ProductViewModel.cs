using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "فیلد نام محصول نمی تواند خالی یاشد.")]
        public string Name { get; set; }

        public int IdCategory { get; set; }

        [Display(Name = "دسته بندی")]
        [Required(ErrorMessage = "فیلد دسته بندی نمی تواند خالی یاشد.")]
        public string CategoryName { get; set; }

        [Display(Name = "کد")]
        [Required(ErrorMessage = "فیلد کد محصول نمی تواند خالی یاشد.")]
        public int Code { get; set; }

        public int IdUnit { get; set; }

        [Display(Name = "واحد")]
        [Required(ErrorMessage = "فیلد واحد نمی تواند خالی یاشد.")]
        public string UnitName { get; set; }

        [Display(Name = "موجودی")]
        [Required(ErrorMessage = "فیلد مقدار موجودی نمی تواند خالی یاشد.")]
        public int Inventory { get; set; } //موجودی

        [Display(Name = " قابل فروش")]
        [Required(ErrorMessage = "فیلد مقدار قابل فروش نمی تواند خالی یاشد.")]
        public bool Issenoble { get; set; } //قابل فروش

        [Display(Name = "نقطه سفارش")]
        [Required(ErrorMessage = "فیلد نقطه سفارش نمی تواند خالی یاشد.")]
        public int OrderPoint { get; set; } //نقطه سفارش

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "فیلد قیمت نمی تواند خالی یاشد.")]
        public int Price { get; set; }

        [Display(Name = "قیمت قدیم")]
        [Required(ErrorMessage = "فیلد قیمت نمی تواند خالی یاشد.")]
        public int OldPrice { get; set; }

        [Display(Name = "عکس")]
       
        public string ImageName { get; set; }

    }
}
