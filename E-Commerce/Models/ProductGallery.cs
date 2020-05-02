using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class ProductGallery
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public string ProductId { get; set; }

		public virtual Product Product { get; set; }

        [Display(Name = "تصویر")]
		public string Image { get; set; }
	}
}