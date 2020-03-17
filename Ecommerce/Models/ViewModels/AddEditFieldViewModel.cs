using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcApplication1.Models;

namespace Ecommerce.Models.ViewModels
{
    public class AddEditFieldViewModel
    {
        public int Id { get; set; }

        [Display(Name = "انتخاب گروه")]
        public int? IdSelectGroup { get; set; }

        public List<SelectGroup> SelectGroupList { get; set; }

        [Display(Name = "انتخاب گروه فیلد")]
        public int IdFieldGroup { get; set; }

        public List<FieldGroup> SelectFiledGroupList { get; set; }

        [Display(Name = "مدل")]
        [Required(ErrorMessage = "فیلد مدل نمی تواند خالی یاشد.")]
        public short Type { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "فیلد عنوان نمی تواند خالی یاشد.")]
        public string Title { get; set; }

        public List<SelectListItem> ICategories { get; set; }

        [Display(Name = "گروهای کالایی")]
        public int[] IdCategory { get; set; }

        public List<FieldType> FildTypeModels { get; set; }

        [Display(Name = "نوع فیلد")]
        public int SelecteFieldTypeId { get; set; }
    }
}
