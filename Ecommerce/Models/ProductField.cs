using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class ProductField
    {
        [Key]
        public int Id { get; set; }

        public int IdProduct { get; set; }
        [ForeignKey("IdProduct")]
        public virtual Product ProductList { get; set; }

        public int IdField { get; set; }
        [ForeignKey("IdField")]
        public virtual Field FieldList { get; set; }

        public string Value { get; set; }
    }
}
