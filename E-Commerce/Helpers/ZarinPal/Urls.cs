namespace ECommerce.Helpers.ZarinPal
{
	public static class Urls
	{
		private const string PaymentReqUrl = "https://{0}zarinpal.com/pg/rest/WebGate/PaymentRequest.json";
		private const string PaymentPgUrl = "https://{0}zarinpal.com/pg/StartPay/{1}/ZarinGate";
		private const string PaymentVerificationUrl = "https://{0}zarinpal.com/pg/rest/WebGate/PaymentVerification.json";
		private const string SandBox = "sandbox.";
		private const string Www = "www.";

		public static string GetPaymentRequestUrl(bool sandBoxMode = false)
		{
			return string.Format(PaymentReqUrl, sandBoxMode ? SandBox : Www);
		}

		public static string GetPaymentGatewayUrl(string authority, bool sandBoxMode = false)
		{
			return string.Format(PaymentPgUrl, sandBoxMode ? SandBox : Www, authority);
		}

		public static string GetVerificationUrl(bool sandBoxMode = false)
		{
			return string.Format(PaymentVerificationUrl, sandBoxMode ? SandBox : Www);
		}
	}
}