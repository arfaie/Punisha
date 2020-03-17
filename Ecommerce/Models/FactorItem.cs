using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class FactorItem
    {
        [Key]
        public int Id { get; set; }

        public int IdFactor { get; set; }
        [ForeignKey("IdFactor")]
        public virtual Factor Factor { get; set; }

        public int IdProduct { get; set; }
        [ForeignKey("IdProduct")]
        public virtual Product Product { get; set; }

        public int UnitCount { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DisCount { get; set; }

        public decimal Tax { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal FinalPrice { get; set; }
    }
}
