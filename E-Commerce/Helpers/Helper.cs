using ECommerce.Data;
using ECommerce.Models;
using ECommerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerce.Helpers
{
	public static class Helper
	{
		public static string MerchantId { get; set; } = "594f1d08-5bfe-11ea-a1d1-000c295eb8fc";
		public static string EmailAddress { get; set; } = "@carbiotic.ir";
		public static string ThumbnailImagePath { get; set; } = "/upload/thumbnailimage/";
		public static string NormalImagePath { get; set; } = "/upload/normalimage/";

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

				return $"{Convert.ToDateTime(persianCalendar.GetYear(dateTime) + "/" + persianCalendar.GetMonth(dateTime) + "/" + persianCalendar.GetDayOfMonth(dateTime)):yyyy/MM/dd}";
			}
			catch (Exception)
			{
				return String.Empty;
			}
		}

		public static string GetGregorianToPersianDate(DateTime dateTime)
		{
			try
			{
				var persianCalendar = new PersianCalendar();
				dateTime = dateTime.ToLocalTime();

				return $"{persianCalendar.GetYear(dateTime)}/{GetPersianMonth(persianCalendar.GetMonth(dateTime))}/{persianCalendar.GetDayOfMonth(dateTime)} {GetPersianDayOfWeek(persianCalendar.GetDayOfWeek(dateTime))}";
			}
			catch (Exception)
			{
				return String.Empty;
			}
		}

		public static string GetGregorianToPersianDateInverse(DateTime dateTime)
		{
			try
			{
				var persianCalendar = new PersianCalendar();
				dateTime = dateTime.ToLocalTime();

				return $"{GetPersianDayOfWeek(persianCalendar.GetDayOfWeek(dateTime))} - {persianCalendar.GetDayOfMonth(dateTime):00} {GetPersianMonth(persianCalendar.GetMonth(dateTime))} {persianCalendar.GetYear(dateTime)}";
			}
			catch (Exception)
			{
				return String.Empty;
			}
		}

		public static string GetPersianYear(DateTime dateTime)
		{
			try
			{
				var persianCalendar = new PersianCalendar();

				return $"{persianCalendar.GetYear(dateTime)}";
			}
			catch (Exception)
			{
				return String.Empty;
			}
		}

		public static string GetPersianMonth(int month)
		{
			switch (month)
			{
				case 1:
					return "فروردین";

				case 2:
					return "اردیبهشت";

				case 3:
					return "خرداد";

				case 4:
					return "تیر";

				case 5:
					return "مرداد";

				case 6:
					return "شهریور";

				case 7:
					return "مهر";

				case 8:
					return "آبان";

				case 9:
					return "آذر";

				case 10:
					return "دی";

				case 11:
					return "بهمن";

				case 12:
					return "اسفند";

				default:
					return month.ToString();
			}
		}

		public static string GetPersianDayOfWeek(DayOfWeek dayOdWeek)
		{
			switch (dayOdWeek)
			{
				case DayOfWeek.Saturday:
					return "شنبه";

				case DayOfWeek.Sunday:
					return "یکشنبه";

				case DayOfWeek.Monday:
					return "دوشنبه";

				case DayOfWeek.Tuesday:
					return "سه شنبه";

				case DayOfWeek.Wednesday:
					return "چهارشنبه";

				case DayOfWeek.Thursday:
					return "پنجشنبه";

				case DayOfWeek.Friday:
					return "جمعه";

				default:
					return String.Empty;
			}
		}

		public static string GetPersianDateAndTimeText(DateTime dateTime)
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

		public static async Task<int> CalculateShippingCostAsync(ApplicationDbContext context, string addressId,
			ICollection<FactorItem> factorItems)
		{
			// get address state,
			var address = await context.Addresses.FirstOrDefaultAsync(x => x.Id == addressId);

			if (address != null)
			{
				var city = await context.Cities.Include(x => x.State).FirstOrDefaultAsync(x => x.Id == address.CityId);

				if (city?.State != null)
				{
					var shippingState =
						await context.ShippingStates.FirstOrDefaultAsync(x => x.Name == city.State.Name);

					// calculate cost
					if (shippingState != null)
					{
						var weight = factorItems.Sum(x => x.UnitCount * x.Product?.Weight);

						if (weight.HasValue)
						{
							var shipping = new ShippingCost(1, shippingState.StateId, weight.Value);

							return (int)shipping.Calculate();
						}
					}
				}
			}

			return 0;
		}
	}
}