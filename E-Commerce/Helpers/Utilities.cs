using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ECommerce.Helpers
{
	public class Utilities
	{
		public int Random(int start, int end)
		{
			var rnd = new Random();
			var random = rnd.Next(start, end);
			return random;
		}

        //public static string ToPersianDateString(DateTime georgianDate)
        //{
        //    System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();

        //    string year = persianCalendar.GetYear(georgianDate).ToString();
        //    string month = persianCalendar.GetMonth(georgianDate).ToString().PadLeft(2, '0');
        //    string day = persianCalendar.GetDayOfMonth(georgianDate).ToString().PadLeft(2, '0');
        //    string persianDateString = String.Format("{0}/{1}/{2}", year, month, day);
        //    return persianDateString;
        //}
    }
}