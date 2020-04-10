using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Address
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "کاربر")]
        public string UserId { get; set; }

        [Display(Name = "کاربر")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "یکی از شهر ها را انتخاب کنید.")]
        public string CityId { get; set; }

        [Display(Name = "شهر")]
        public virtual City City { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "فیلد {0} نمی تواند خالی باشد.")]
        public string Description { get; set; }

        [Display(Name = "کدپستی")]
        [Required(ErrorMessage = "کد پستی را وارد نمایید.")]
        public string PostalCode { get; set; }

        [Display(Name = "همراه")]
        [Required(ErrorMessage = "شماره همراه را وارد نمایید.")]
        public string Mobile { get; set; }

        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "شماره تلفن را وارد نمایید")]
        public string Phone { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public double Latitude { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        public double Longitude { get; set; }

        [Display(Name = "تحویل گیرنده")]
        [Required(ErrorMessage = "نام تحویل گیرنده را وارد نمایید.")]
        public string Recipient { get; set; }

        public ICollection<Factor> Factors { get; set; }
    }
}