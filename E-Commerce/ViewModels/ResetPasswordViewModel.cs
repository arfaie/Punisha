using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required]
		[Phone]
		public string Mobile { get; set; }

		[StringLength(100, ErrorMessage = "{0} باید دارای حداقل {2} حرف یا عدد باشد.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "رمز عبور")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "تکرار رمز عبور")]
		[Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشد.")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}
}