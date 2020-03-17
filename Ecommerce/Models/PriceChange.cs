using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class PriceChange
    {
        [Key]
        public int Id { get; set; }

        public int IdProduct { get; set; }

        [ForeignKey("IdProduct")]
        public virtual Product Product { get; set; }

        public int Old { get; set; }

        public int New { get; set; }

        public DateTime Date { get; set; }
    }
}
