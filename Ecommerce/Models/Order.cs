using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int IdFactor { get; set; }
        [ForeignKey("IdFactor")]
        public virtual Factor Factor { get; set; }

        public long TransactionNUmber { get; set; }

        public bool TransactionStatus { get; set; }

        public DateTime TransactionDate { get; set; }

        public int IdStatus { get; set; }
        [ForeignKey("IdStatus")]
        public virtual Status Status { get; set; }

        public long IssuCode { get; set; }

        public string Des { get; set; }


    }
}
