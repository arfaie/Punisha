using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class CarProduct
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "مدل خودرو")]
		public string CarId { get; set; }

		[Display(Name = "مدل خودرو")]
		public virtual Car Car { get; set; }

		[Display(Name = "محصول")]
		public string ProductId { get; set; }

		[Display(Name = "محصول")]
		public virtual Product Product { get; set; }
	}
}