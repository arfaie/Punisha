using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class AddEditSelectItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "انتخاب آیتم")]
        public int IdSelectGroup { get; set; }

        public List<SelectListItem> SelectGroupListItems { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }
    }
}
