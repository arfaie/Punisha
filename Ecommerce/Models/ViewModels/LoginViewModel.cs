using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "نام کاربری:")]
        [Required(ErrorMessage = "لطفا نام کاربری را وارد نمایید")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور :")]
        [Required(ErrorMessage = "لطفا رمز عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا بخاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
