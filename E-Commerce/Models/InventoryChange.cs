using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class InventoryChange
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "کالا")]
		public string ProductId { get; set; }

		[Display(Name = "کالا")]
		public virtual Product Product { get; set; }

		[Display(Name = "قیمت قبلی")]
		public int Old { get; set; }

		[Display(Name = "قیمت جدید")]
		public int New { get; set; }

		[Display(Name = "تاریخ")]
		public DateTime Date { get; set; }
	}
}