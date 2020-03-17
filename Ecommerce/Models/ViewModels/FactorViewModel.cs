using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class FactorViewModel
    {
        public int Id { get; set; }

        public string IdUser { get; set; }
        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "نام و نام خانوادگی کاربر")]
        public string UserFullName { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalDisCount { get; set; }

        public decimal Tax { get; set; }

        public decimal FinalPrice { get; set; }

        public DateTime Date { get; set; }

        public long FactorCode { get; set; }
    }
}
