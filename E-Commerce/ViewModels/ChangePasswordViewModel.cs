using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class ChangePasswordViewModel
	{
		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[DataType(DataType.Password)]
		[Display(Name = "رمز عبور فعلی")]
		public string OldPassword { get; set; }

		[StringLength(100, ErrorMessage = "{0} باید دارای حداقل {2} حرف یا عدد باشد.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "رمز عبور جدید")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "تکرار رمز عبور")]
		[Compare(nameof(NewPassword), ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشد.")]
		public string ConfirmPassword { get; set; }
	}
}