using System;
using System.ComponentModel.DataAnnotations;


namespace Ecommerce.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "فیلد عنوان نمیتواند خالی باشد.")]
        public string Title { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        public DateTime EndDate { get; set; }

        [Display(Name = "فعال")]
        public bool IsActiv { get; set; }
    }
}
