using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[Phone]
		[StringLength(11, ErrorMessage = "شماره موبایل باید 11 عدد باشد", MinimumLength = 11)]
		[Display(Name = "شماره موبایل")]
		[Remote(action: "VerifyMobile", controller: "Account")]
		public string Mobile { get; set; }

		[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
		[DataType(DataType.Password)]
		[Display(Name = "رمز عبور")]
		public string Password { get; set; }
	}
}