using System.ComponentModel.DataAnnotations;


namespace Ecommerce.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عکس")]
        public string Image { get; set; }
    }
}
