using Newtonsoft.Json;

namespace ECommerce.Helpers.ZarinPal
{
	public static class ZarinPalPayment
	{
		private const int AuthorityLength = 36;

		private const bool IsSandbox = false;

		public static PayResponse Request(long amount, string description, string callbackUrl)
		{
			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			var httpCore = new HttpCore(Urls.GetPaymentRequestUrl(IsSandbox), "POST",
				JsonConvert.SerializeObject(new PayRequest(Helper.MerchantId, amount, description, callbackUrl)));
			var res = JsonConvert.DeserializeObject<PayResponse>(httpCore.GetResponse());
			res.Authority = res.Authority.TrimStart('0');
			return res;
		}

		public static PayVerifyResponse Verify(long amount, string authority)
		{
			var z = "";
			var count = AuthorityLength - authority.Length;
			for (var i = 0; i < count; i++) z += "0";
			authority = z + authority;

			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			var httpCore = new HttpCore(Urls.GetVerificationUrl(IsSandbox), "POST",
				JsonConvert.SerializeObject(new PayVerify(Helper.MerchantId, amount, authority)));
			return JsonConvert.DeserializeObject<PayVerifyResponse>(httpCore.GetResponse());
		}

		public static string GetPaymentGatewayUrl(string authority)
		{
			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			return Urls.GetPaymentGatewayUrl(authority, IsSandbox);
		}
	}
}