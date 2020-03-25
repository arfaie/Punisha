using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Address
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "کاربر")]
		public string UserId { get; set; }

		[Display(Name = "کاربر")]
		public virtual ApplicationUser User { get; set; }

		[Display(Name = "شهر")]
		public string CityId { get; set; }

		[Display(Name = "شهر")]
		public virtual City City { get; set; }

		[Display(Name = "توضیحات")]
		public string Description { get; set; }

		[Display(Name = "کدپستی")]
		public string PostalCode { get; set; }

		[Display(Name = "همراه")]
		public string Mobile { get; set; }

		[Display(Name = "تلفن")]
		public string Phone { get; set; }

		[Display(Name = "طول جغرافیایی")]
		public double Latitude { get; set; }

		[Display(Name = "عرض جغرافیایی")]
		public double Longitude { get; set; }

		[Display(Name = "تحویل گیرنده")]
		public string Recipient { get; set; }
	}
}