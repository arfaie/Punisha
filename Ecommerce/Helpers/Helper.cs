using System;
using System.Globalization;

namespace Ecommerce.Helpers
{
	public static class Helper
	{
		public static string MerchantId { get; set; } = "594f1d08-5bfe-11ea-a1d1-000c295eb8fc";

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
	}
}