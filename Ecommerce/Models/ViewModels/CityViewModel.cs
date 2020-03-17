using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class CityViewModel
    {
        
        public int Id { get; set; }

        [Display(Name = "نام شهر")]
        [Required(ErrorMessage = "لطفا نام شهر را وارد نمایید.")]
        public string Name { get; set; }

        public int StatesId { get; set; }

        [Display(Name = "نام استان")]
        public string StateName { get; set; }
    }
}
