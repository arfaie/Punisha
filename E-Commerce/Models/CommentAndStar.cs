using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class CommentAndStar
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

		[Display(Name = "نظر")]
		public string Comment { get; set; }

		[Display(Name = "ستارخ")]
		public int Starts { get; set; }
	}
}