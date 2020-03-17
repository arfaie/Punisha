using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class ChangePassViewModel
    {
        public string Id { get; set; }

        [Display(Name = "رمز عبور فعلی")]
        [Required(ErrorMessage = "لطفا رمز عبور فعلی را وارد کنید")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "حداق طول رمز عبور 6 کاراکتر و حداکثر 20 کاراکتر می باشد")]
        public string OldPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا رمز عبور جدید را وارد کنید")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "حداق طول رمز عبور 6 کاراکتر و حداکثر 20 کاراکتر می باشد")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "رمز عبور باید ترکیبی از حروف کوچک و بزرگ و عدد و علامت باشد.")]
        public string NewPassword { get; set; }

        [Display(Name = " تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا تکرار رمز عبور جدید را وارد کنید")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رمز عبور با تکرار آن یکسان نیست!")]
        public string ConfirmNewPassword { get; set; }
    }
}
