using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class AddEditProductGallery
    {
        public int Id { get; set; }

        [Display(Name = "عکس")]
        public string Image { get; set; }


        /// /////////////////////////////////////////////////////


        [Display(Name = "خبر")]
        public int IdProduct { get; set; }

        public List<SelectListItem> ProductListItems { get; set; }
    }
}
