using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class ShippingState
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "شناسه استان")]
		public int StateId { get; set; }

		[Display(Name = "نام استان")]
		public string Name { get; set; }
	}
}