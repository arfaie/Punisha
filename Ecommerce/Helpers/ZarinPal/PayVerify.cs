namespace Ecommerce.Helpers.ZarinPal
{
	public class PayVerify
	{
		public PayVerify(string merchantId, long amount, string authority)
		{
			MerchantID = merchantId;
			Amount = amount;
			Authority = authority;
		}

		public string MerchantID { get; set; }
		public long Amount { get; set; }
		public string Authority { get; set; }
	}
}