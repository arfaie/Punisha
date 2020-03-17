using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class AddEditCityViewModel
    {
        public int Id { get; set; }

        [Display(Name = "نام شهر")]
        [Required(ErrorMessage = "لطفا نام شهر را وارد نمایید.")]
        public string Name { get; set; }

        [Display(Name = "نام استان")]
        public int StatesId { get; set; }

        public List<SelectListItem> StaSelectListItems { get; set; }
    }
}
