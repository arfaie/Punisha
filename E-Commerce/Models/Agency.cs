using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Agency
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "عنوان")]
		[Required(ErrorMessage = "فیلد {0} نمی تواند خالی یاشد.")]
		public string Title { get; set; }

		[Display(Name = "نام و نام خانوادگی")]
		public string FullName { get; set; }

		[Display(Name = "شهر")]
		public string CityId { get; set; }

		[Display(Name = "شهر")]
		public virtual City City { get; set; }

		[Display(Name = "آدرس")]
		[Required(ErrorMessage = "فیلد {0} نمی تواند خالی یاشد.")]
		public string Address { get; set; }

		[Display(Name = "کدپستی")]
		public string PostalCode { get; set; }

		[Display(Name = "تلفن")]
		[Required(ErrorMessage = "فیلد {0} نمی تواند خالی یاشد.")]
		public string Phone { get; set; }
	}
}