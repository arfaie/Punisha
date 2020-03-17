using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class FieldsViewModel
    {

        public int IdField { get; set; }
        public int IdFieldGroup { get; set; }
        public int idPrCatField { get; set; }
        public int idProductField { get; set; }
        public int idProduct { get; set; }
        public int idCar { get; set; }
        public int[] idCars { get; set; }
        public short Type { get; set; }
        public string FieldTitle { get; set; }
        public string FieldGroTitle { get; set; }

        public int IdCategory { get; set; }

        public string Value { get; set; }
        public string ProductName { get; set; }
        public string CarName { get; set; }
        public string ProductCategory { get; set; }
        public string CarNames { get; set; }

        

        //public int[] IdFields { get; set; }
        //public int IdProduct { get; set; }
        //public int IdProductField { get; set; }
        ////public int IdField { get; set; }
        ////public int IdCategory { get; set; }
        //public int IdFieldGroup { get; set; }

        

        //[Display(Name = "دسته بندی")]
        //[Required(ErrorMessage = "فیلد عنوان نمی تواند خالی یاشد.")]
        //public string fieldGroup { get; set; }

        //public List<int> lsFieldGroup { get; set; }

        //public List<SelectListItem> CarListItems { get; set; }
        ////public List<CarIDModel> carIDModels { get; set; }

        //[Display(Name = "ماشین")]
        //public int[] CarId { get; set; }
        
        //public string CarNames { get; set; }
        //public int IdItem { get; set; }

        

        ////public List<FieldsModel> FieldsModels { get; set; }



        ////[Display(Name = "دسته بندی")]
        ////[Required(ErrorMessage = "فیلد دسته بندی نمی تواند خالی یاشد.")]
        ////public string CategoryName { get; set; }

        ////[Display(Name = "کد")]
        ////[Required(ErrorMessage = "فیلد کد محصول نمی تواند خالی یاشد.")]
        ////public int Code { get; set; }

        ////public int IdUnit { get; set; }

        ////[Display(Name = "واحد")]
        ////[Required(ErrorMessage = "فیلد واحد نمی تواند خالی یاشد.")]
        ////public string UnitName { get; set; }

        ////[Display(Name = "موجودی")]
        ////[Required(ErrorMessage = "فیلد مقدار موجودی نمی تواند خالی یاشد.")]
        ////public int Inventory { get; set; } //موجودی

        ////[Display(Name = " قابل فروش")]
        ////[Required(ErrorMessage = "فیلد مقدار قابل فروش نمی تواند خالی یاشد.")]
        ////public bool Issenoble { get; set; } //قابل فروش

        ////[Display(Name = "نقطه سفارش")]
        ////[Required(ErrorMessage = "فیلد نقطه سفارش نمی تواند خالی یاشد.")]
        ////public int OrderPoint { get; set; } //نقطه سفارش

        ////[Display(Name = "قیمت")]
        ////[Required(ErrorMessage = "فیلد قیمت نمی تواند خالی یاشد.")]
        ////public decimal Price { get; set; }

        ////[Display(Name = "عکس")]

        ////public string ImageName { get; set; }

    }

    //public class CarIDModel
    //{
    //    public int ID { get; set; }
    //    public int IdProductField { get; set; }
    //    public int[] CarId { get; set; }
    //    //public string Value { get; set; }
    //}

}
