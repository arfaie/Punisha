using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class FieldViewModel
    {
        public int Id { get; set; }
        public int IdType { get; set; }
        
        //public int IdSelectGroup { get; set; }

        [Display(Name = "گروه های کالایی")]
        public string SelectGroupName { get; set; }

        public int IdFieldGroup { get; set; }

        [Display(Name = "گروه فیلد")]
        public string SelectFieldGroupName { get; set; }

        [Display(Name = "نوع")]
        public string Type { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }
    }
}
