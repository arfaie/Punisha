using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class AdminChangePasswordViewModel
	{
		public string Id { get; set; }

		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[DataType(DataType.Password)]
		[Display(Name = "رمز عبور فعلی")]
		public string OldPassword { get; set; }

		[StringLength(100, ErrorMessage = "{0} باید دارای حداقل {2} حرف یا عدد باشد.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "رمز عبور جدید")]
		public string NewPassword { get; set; }
	}
}