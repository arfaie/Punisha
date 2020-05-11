using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class BlogComment
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserId  { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string BlogId { get; set; }

        [ForeignKey("BlogId")]
        public virtual News News { get; set; }

        [Display(Name = "متن")]
        public string Comment { get; set; }

        public bool Accepted { get; set; }
    }
}
