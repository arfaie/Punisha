using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Brand
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "عنوان")]
		[Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد.")]
		public string Title { get; set; }

		public ICollection<Product> Products { get; set; }

	}
}