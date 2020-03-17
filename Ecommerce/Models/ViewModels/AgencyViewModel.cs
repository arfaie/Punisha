using System.ComponentModel.DataAnnotations;


namespace Ecommerce.Models.ViewModels
{
    public class AgencyViewModel
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

        public int IdState { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "فیلد استان نمی تواند خالی یاشد.")]
        public string StateName { get; set; }

        public int IdCity { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "فیلد شهر نمی تواند خالی یاشد.")]
        public string CityName { get; set; }

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
