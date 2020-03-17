using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا عنوان نقش را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "توضیحات نقش")]
        [Required(ErrorMessage = "لطفا توضیحات نقش را وارد کنید")]
        public string Description { get; set; }
    }
}
