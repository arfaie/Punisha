namespace Ecommerce.Helpers.ZarinPal
{
	public class PayRequest
	{
		public PayRequest(string merchantId, long amount, string description, string callbackUrl)
		{
			MerchantID = merchantId;
			Amount = amount;
			Description = description;
			CallbackURL = callbackUrl;
		}

		public string MerchantID { get; set; }
		public long Amount { get; set; }
		public string Description { get; set; }
		public string CallbackURL { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
	}
}