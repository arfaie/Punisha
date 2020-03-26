using ECommerce.Models.Helpers.OptionEnums;
using System;
using System.ComponentModel;

namespace ECommerce.Models.Helpers
{
	public static class Notification
	{
		#region HELPERS

		private static string StringValueOf(Enum value)
		{
			var fi = value.GetType().GetField(value.ToString());
			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0)
			{
				return attributes[0].Description;
			}

			return value.ToString();
		}

		private static string BooToLowerString(this bool value)
		{
			return value.ToString().ToLower();
		}

		#endregion HELPERS

		public static string ShowNotif(string message,
			ToastType type)
		{
			string color;
			var iconColor = "'#ABEBC6'";
			if (type == ToastType.Green)
			{
				color = "'#1ABC9C'";
			}
			else if (type == ToastType.Red)
			{
				color = "'#FF5733'";
				iconColor = "'snow'";
			}
			else if (type == ToastType.Yellow)
			{
				color = "'#F4D03F'";
			}
			else if (type == ToastType.Green2)
			{
				color = "'#196F3D'";
			}
			else
			{
				color = "'#5DADE2'";
			}

			var scriptOption = "<script>";
			scriptOption += "iziToast.show({";
			scriptOption += "message:'" + message + "'" + ",theme: 'light', color: '" + type + "',icon: 'icon icon-check', iconColor:" + iconColor + ",messageColor: 'snow',backgroundColor: " + color + ",maxWidth: 500,layout: 2,balloon: false,close: true,closeOnEscape: true,closeOnClick: true,displayMode: 0, position: 'topLeft',targetFirst: true,timeout: 5000,rtl: true,animateInside: true,drag: true,pauseOnHover: true,resetOnHover: false,progressBar: true,progressBarColor: '',progressBarEasing: 'linear',overlay: false,overlayClose: false,overlayColor: 'rgba(0, 0, 0, 0.6)',transitionIn: 'fadeInUp',transitionOut: 'fadeOut',transitionInMobile: 'fadeInUp',transitionOutMobile: 'fadeOutDown'";
			scriptOption += "});";
			scriptOption += "</script>";

			return scriptOption;
		}
	}
}