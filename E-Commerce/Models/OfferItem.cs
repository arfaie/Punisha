using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class OfferItem
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "تخفیف")]
		public string OfferId { get; set; }

		[Display(Name = "تخفیف")]
		public virtual Offer Offer { get; set; }

		[Display(Name = "کالا")]
		public string ProductId { get; set; }

		[Display(Name = "کالا")]
		public virtual Product Product { get; set; }

		[Display(Name = "مقدار تخفیف")]
		public float DiscountAmount { get; set; }

		[Display(Name = "درصد تخفیف")]
		public float DiscountPercent { get; set; }
	}
}