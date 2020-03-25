using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Offer
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "عنوان")]
		[Required(ErrorMessage = "فیلد عنوان نمیتواند خالی باشد.")]
		public string Title { get; set; }

		[Display(Name = "تاریخ شروع")]
		public DateTime StartDate { get; set; }

		[Display(Name = "تاریخ پایان")]
		public DateTime EndDate { get; set; }

		[Display(Name = "فعال")]
		public bool IsActive { get; set; }

		public ICollection<OfferItem> OfferItems { get; set; }
	}
}