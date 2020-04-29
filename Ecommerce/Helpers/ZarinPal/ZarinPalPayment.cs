using Newtonsoft.Json;

namespace Ecommerce.Helpers.ZarinPal
{
	public static class ZarinPalPayment
	{
		private const int AUTHORITY_LENGTH = 36;

		private const bool IS_SANDBOX = false;

		public static PayResponse Request(long amount, string description, string callbackUrl)
		{
			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			HttpCore httpCore = new HttpCore(Urls.GetPaymentRequestUrl(IS_SANDBOX), "POST",
				JsonConvert.SerializeObject(new PayRequest(Helper.MerchantId, amount, description, callbackUrl)));
			var res = JsonConvert.DeserializeObject<PayResponse>(httpCore.GetResponse());
			res.Authority = res.Authority.TrimStart('0');
			return res;
		}

		public static PayVerifyResponse Verify(long amount, string authority)
		{
			string z = "";
			int count = AUTHORITY_LENGTH - authority.Length;
			for (var i = 0; i < count; i++) z += "0";
			authority = z + authority;

			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			HttpCore httpCore = new HttpCore(Urls.GetVerificationUrl(IS_SANDBOX), "POST",
				JsonConvert.SerializeObject(new PayVerify(Helper.MerchantId, amount, authority)));
			return JsonConvert.DeserializeObject<PayVerifyResponse>(httpCore.GetResponse());
		}

		public static string GetPaymentGatewayUrl(string authority)
		{
			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			return Urls.GetPaymentGatewayUrl(authority, IS_SANDBOX);
		}
	}
}