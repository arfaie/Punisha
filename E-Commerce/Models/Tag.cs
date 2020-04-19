using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECommerce.Models
{
    public class Tag
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "عنوان تگ")]
        public string Title { get; set; }
    }
}
