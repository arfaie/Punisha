using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class SelectItem
    {
        public int Id { get; set; }

        public int IdSelectGroup { get; set; }
        [ForeignKey("IdSelectGroup")]
        public virtual SelectGroup SelectGroupList { get; set; }

        public string Title { get; set; }
    }
}
