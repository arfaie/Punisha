using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[EmailAddress]
		public string Email { get; set; }
	}
}