using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	// تاریخچه بازدید کاربر از محصولات
	public class History
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "کاربر")]
		public string UserId { get; set; }

		[Display(Name = "کاربر")]
		public virtual ApplicationUser User { get; set; }

		[Display(Name = "کالا")]
		public string ProductId { get; set; }

		[Display(Name = "کالا")]
		public virtual Product Product { get; set; }

		[Display(Name = "زمان بازدید")]
		public DateTime RegistrationDateAndTime { get; set; }
	}
}