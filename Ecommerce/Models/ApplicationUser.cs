using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public string Mobile { get; set; }

		public long NatCode { get; set; }

		public string TaxiCode { get; set; }

		public int IdCar { get; set; }

		[ForeignKey("IdCar")]
		public virtual Car Cars { get; set; }

		public int IdUserGroup { get; set; }

		[ForeignKey("IdUserGroup")]
		public virtual UserGroup UserGroup { get; set; }
	}
}