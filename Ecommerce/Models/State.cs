using System.ComponentModel.DataAnnotations;


namespace Ecommerce.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }
    }
}
