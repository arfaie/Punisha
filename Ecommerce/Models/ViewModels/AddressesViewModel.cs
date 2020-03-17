using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class AddressesViewModel
    {
        public int Id { get; set; }

        public string IdUser { get; set; }
        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "نام و نام خانوادگی کاربر")]
        public string UserFullName { get; set; }

        public int IdState { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "فیلد استان نمی تواند خالی یاشد.")]
        public string StateName { get; set; }

        public int IdCity { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "فیلد شهر نمی تواند خالی یاشد.")]
        public string CityName { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "پلاک")]
        public string Plaque { get; set; }

        [Display(Name = "کد پستی")]
        public long PostalCode { get; set; }

        [Display(Name = "تحویل گیرنده")]
        public string Transferee { get; set; }

        [Display(Name="همراه")]
        public long Mobile { get; set; }

        [Display(Name = "تلفن ثابت")]
        public long Tell { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public double Lat { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        public double Lan { get; set; }
    }
}
