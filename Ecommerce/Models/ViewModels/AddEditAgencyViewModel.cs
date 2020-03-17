using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Ecommerce.Models.ViewModels
{
    public class AddEditAgencyViewModel
    {
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "فیلد عنوان نمی تواند خالی یاشد.")]
        public string Title { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "فیلد نام نمی تواند خالی یاشد.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "فیلد نام خانوادگی نمی تواند خالی یاشد.")]
        public string LastName { get; set; }

        [Display(Name = "استان")]
        public int IdState { get; set; }

        public List<SelectListItem> StatesList { get; set; }

        [Display(Name = "شهر")]
        public int IdCity { get; set; }

        public List<SelectListItem> CitiesList { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "فیلد آدرس نمی تواند خالی یاشد.")]
        public string Address { get; set; }

        [Display(Name = "پلاک")]
        [Required(ErrorMessage = "فیلد پلاک نمی تواند خالی یاشد.")]
        public string Plaque { get; set; }

        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "فیلد کد پستی نمی تواند خالی یاشد.")]
        public long PostalCode { get; set; }

        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "فیلد تلفن نمی تواند خالی یاشد.")]
        public long Tell { get; set; }
    }
}
