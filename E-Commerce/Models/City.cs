using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class City
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "نام شهر")]
		[Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
		public string Name { get; set; }

		[Display(Name = "استان")]
		public string StateId { get; set; }

		[Display(Name = "استان")]
		public virtual State State { get; set; }

		public ICollection<Address> Addresses { get; set; }

		public ICollection<Agency> Agencies { get; set; }
	}
}