using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class Factor
    {
        [Key]
        public int Id { get; set; }

        public string IdUser { get; set; }
        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalDisCount { get; set; }

        public decimal Tax { get; set; }

        public decimal FinalPrice { get; set; }

        public DateTime Date { get; set; }

        public string FactorCode { get; set; }

        public bool IsPayed { get; set; }

    }
}
