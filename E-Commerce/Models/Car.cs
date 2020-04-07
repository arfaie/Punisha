using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Car
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "نام")]
		[Required(ErrorMessage = "فیلد {0} نمی تواند خالی باشد.")]
		public string Name { get; set; }

		[Display(Name = "نوع خودرو")]
		public string MakerId { get; set; }

		[Display(Name = "نوع خودرو")]
		public virtual Maker Maker { get; set; }

		public ICollection<ApplicationUser> ApplicationUsers { get; set; }

		public ICollection<CarProduct> CarProducts { get; set; }
	}
}