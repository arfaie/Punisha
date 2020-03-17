using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class SelectGroup
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "فیلد عنوان نمی تواند خالی باشد.")]
        public string Title { get; set; }
    }
}
