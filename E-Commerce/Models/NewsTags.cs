using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class NewsTags
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "خبر")]
        public string IdNews { get; set; }

        [ForeignKey("IdNews")]
        public virtual News news { get; set; }

        [Display(Name = "تگ")]
        public string IdTag { get; set; }

        [ForeignKey("IdTag")]
        public virtual Tag tags { get; set; }
    }
}
