using ECommerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ECommerce.Helpers
{
	public static class Helper
	{
		public static string MerchantId { get; set; } = "594f1d08-5bfe-11ea-a1d1-000c295eb8fc";
		public static string EmailAddress { get; set; } = "@carbiotic.ir";

		private static readonly List<ConfirmationCodeViewModel> ConfirmationCodes = new List<ConfirmationCodeViewModel>();

		public static int GenerateShortenCode(string mobile, string link)
		{
			var number = new Random().Next(10001, 99999);

			ConfirmationCodes.Add(new ConfirmationCodeViewModel { Mobile = mobile, Link = link, ShortenCode = number.ToString() });

			return number;
		}

		public static string GetLink(string mobile, string shortCode, bool shouldRemoveCode = true)
		{
			var link = String.Empty;

			var code = ConfirmationCodes.FirstOrDefault(x => x.Mobile == mobile && x.ShortenCode == shortCode);

			if (code != null)
			{
				link = code.Link;

				if (shouldRemoveCode)
				{
					ConfirmationCodes.Remove(code);
				}
			}

			return link;
		}

		public static string GetPersianDateText(DateTime dateTime)
		{
			try
			{
				var persianCalendar = new PersianCalendar();
				dateTime = dateTime.ToLocalTime();

				return $"{Convert.ToDateTime(persianCalendar.GetYear(dateTime) + "/" + persianCalendar.GetMonth(dateTime) + "/" + persianCalendar.GetDayOfMonth(dateTime)):yyyy/MM/dd}|{Convert.ToDateTime(dateTime.TimeOfDay.Hours + ":" + dateTime.TimeOfDay.Minutes + ":" + dateTime.TimeOfDay.Seconds):HH:mm:ss}";
			}
			catch (Exception)
			{
				return String.Empty;
			}
		}

		public static bool IsMobileNumberValid(string input)
		{
			if (String.IsNullOrWhiteSpace(input))
			{
				return false;
			}

			input = GetEnglishStringNumber(input);

			var check = new Regex(@"^09\d{9}$");
			return check.IsMatch(input);
		}

		public static string GetEnglishStringNumber(string value)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				return String.Empty;
			}

			value = value.Replace(",", String.Empty).Replace("/", ".").Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");

			return value;
		}
	}
}