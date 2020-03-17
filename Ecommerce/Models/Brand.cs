using System.ComponentModel.DataAnnotations;


namespace Ecommerce.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="عنوان")]
        [Required(ErrorMessage = "فیلد عنوان نمیتواند خالی باشد.")]
        public string Title { get; set; }
    }
}
