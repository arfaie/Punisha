using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class ConfirmationCodeViewModel
	{
		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[Display(Name = "شماره موبایل")]
		public string Mobile { get; set; }

		public string Link { get; set; }

		[Required(ErrorMessage = "وارد کردن کد الزامی است")]
		public string ShortenCode { get; set; }

		public bool IsConfirmed { get; set; }

		public bool IsResetPassword { get; set; }
	}
}