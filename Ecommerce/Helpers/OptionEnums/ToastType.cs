using System.ComponentModel;

namespace Ecommerce.Helpers.OptionEnums
{
	public enum ToastType
	{
		//blue, red, green, yellow
		[Description("success")]
		green,

		[Description("info")]
		blue,

		[Description("warning")]
		yellow,

		[Description("error")]
		red,

		[Description("success2")]
		green2,
	}
}