namespace ECommerce.Helpers.ZarinPal
{
	public class PayRequest
	{
		public PayRequest(string merchantId, long amount, string description, string callbackUrl)
		{
			MerchantId = merchantId;
			Amount = amount;
			Description = description;
			CallbackUrl = callbackUrl;
		}

		public string MerchantId { get; set; }
		public long Amount { get; set; }
		public string Description { get; set; }
		public string CallbackUrl { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
	}
}