using Newtonsoft.Json;

namespace Ecommerce.Helpers.ZarinPal
{
	public class PayVerifyResponse
	{
		public bool IsSuccess => Status == 100 || Status == 101;
		public string RefId { get; set; }
		public int Status { get; set; }
		public object Errors { get; set; }
		public string ErrorsStr => JsonConvert.SerializeObject(Errors);
	}
}