using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class VerifyPhoneNumberViewModel
	{
		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		public string Code { get; set; }

		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[Phone]
		[Display(Name = "شماره موبایل")]
		public string PhoneNumber { get; set; }
	}
}