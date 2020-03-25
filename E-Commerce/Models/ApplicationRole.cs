using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models
{
	public class ApplicationRole : IdentityRole
	{
		public string Description { get; set; }
	}
}