namespace ECommerce.Helpers.ZarinPal
{
	public class PayVerify
	{
		public PayVerify(string merchantId, long amount, string authority)
		{
			MerchantId = merchantId;
			Amount = amount;
			Authority = authority;
		}

		public string MerchantId { get; set; }
		public long Amount { get; set; }
		public string Authority { get; set; }
	}
}