using System.Collections.Generic;

namespace ECommerce.ViewModels
{
	public class SmsViewModel
	{
		public class Return
		{
			public int Status { get; set; }
			public string Message { get; set; }
		}

		public class Entry
		{
			public int Messageid { get; set; }
			public string Message { get; set; }
			public int Status { get; set; }
			public string Statustext { get; set; }
			public string Sender { get; set; }
			public string Receptor { get; set; }
			public int Date { get; set; }
			public int Cost { get; set; }
		}

		public class RootObject
		{
			public Return Return { get; set; }
			public List<Entry> Entries { get; set; }
		}
	}
}