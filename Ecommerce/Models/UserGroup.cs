using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "فیلد عنوان نمی تواند خالی باشد.")]
        public string Title { get; set; }
    }
}
