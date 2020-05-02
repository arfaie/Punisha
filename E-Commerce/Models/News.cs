using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class News
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "متن")]
        public string Text { get; set; }

        [Display(Name = "دسته بندی")]
        public string IdCategories { get; set; }

        [ForeignKey("IdCategories")]
        public virtual NewsCategory NewCategories { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        public ICollection<NewsTags> NewsTagses { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
