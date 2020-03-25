using Newtonsoft.Json;

namespace ECommerce.Helpers.ZarinPal
{
	public class PayResponse
	{
		public bool IsSuccess => Status == 100 || Status == 101;
		public string Authority { get; set; }
		public int Status { get; set; }
		public object Errors { get; set; }
		public string ErrorsStr => JsonConvert.SerializeObject(Errors);
	}
}