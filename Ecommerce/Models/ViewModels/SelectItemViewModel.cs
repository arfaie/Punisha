using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class SelectItemViewModel
    {
        public int Id { get; set; }

        public int IdSelectGroup { get; set; }

        [Display(Name = "گروه آیتم")]
        public string SelectGroupTitle { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }
    }
}
