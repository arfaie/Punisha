using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class FactorItem
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "فاکتور")]
		public string FactorId { get; set; }

		[Display(Name = "فاکتور")]
		public virtual Factor Factor { get; set; }

		[Display(Name = "کالا")]
		public string ProductId { get; set; }

		[Display(Name = "کالا")]
		public virtual Product Product { get; set; }

		[Display(Name = "تعداد")]
		public int UnitCount { get; set; }

		[Display(Name = "قیمت واحد")]
		public int UnitPrice { get; set; }

		[Display(Name = "تخفیف")]
		public float Discount { get; set; }
	}
}