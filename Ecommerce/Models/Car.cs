using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "فیلد نام نمی تواند خالی باشد.")]
        public string Name { get; set; }
    }
}
