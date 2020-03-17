using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام شهر")]
        [Required(ErrorMessage = "لطفا نام شهر را وارد نمایید.")]
        public string Name { get; set; }

        public int StatesId { get; set; }
        [ForeignKey("StatesId")]
        public virtual State States { get; set; }
    }
}
