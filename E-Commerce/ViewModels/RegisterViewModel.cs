using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[Phone]
		[StringLength(11, ErrorMessage = "شماره موبایل باید 11 عدد باشد", MinimumLength = 11)]
		[Display(Name = "شماره موبایل")]
		[Remote("VerifyMobile", "Account")]
		public string Mobile { get; set; }

		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		//[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[StringLength(100, ErrorMessage = "{0} باید دارای حداقل {2} حرف یا عدد باشد.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "رمز عبور")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "تکرار رمز عبور")]
		[Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشد.")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "پذیرفتن قوانین و شرایط")]
		public bool IsTermsAccepted { get; set; }

		public bool IsShop { get; set; }
	}
}