using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class ProductSelectedItems
    {
        [Key]
        public int Id { get; set; }

        public int IdProductField { get; set; }
        [ForeignKey("IdProductField")]
        public virtual ProductField ProductFieldList { get; set; }

        public int IdItem { get; set; }
        [ForeignKey("IdItem")]
        public virtual SelectItem ItemList { get; set; }

    }
}
