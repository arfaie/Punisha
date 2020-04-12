using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Factor
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "کاربر")]
		public string UserId { get; set; }

		[Display(Name = "کاربر")]
		public virtual ApplicationUser User { get; set; }

        [Display(Name = "آدرس ارسال شده")]
        public string AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

		[Display(Name = "جمع مبلغ")]
		public int TotalPrice { get; set; }

		[Display(Name = "جمع تخفیف")]
		public int TotalDiscount { get; set; }

		[Display(Name = "مالیات")]
		public float Tax { get; set; }

		[Display(Name = "مبلغ نهایی")]
		public int FinalPrice { get; set; }

		[Display(Name = "تاریخ")]
		public DateTime Date { get; set; }

		[Display(Name = "کد فاکتور")]
		public string FactorCode { get; set; }

		[Display(Name = "پرداخت شده")]
		public bool IsPaid { get; set; }

		[Display(Name = "آدرس تحویل")]
		public string AddressId { get; set; }

		[Display(Name = "آدرس تحویل")]
		public virtual Address Address { get; set; }

		public ICollection<FactorItem> FactorItems { get; set; }

		public ICollection<Order> Orders { get; set; }
	}
}