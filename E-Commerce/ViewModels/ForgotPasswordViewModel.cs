using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[Phone]
		[Display(Name = "شماره موبایل")]
		public string Mobile { get; set; }

		public bool IsResetPassword { get; set; }
	}
}