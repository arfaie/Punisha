using ECommerce.Models.Helpers.OptionEnums;
using ECommerce.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ECommerce.Helpers
{
	public class SendSms
	{
		private string _baseurl = "https://api.kavenegar.com/";

		public async Task<bool> SendAsync(short type, string mobile, string token, string token2 = null)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_baseurl);

				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage res = null;

				if (type == (int)SmsTypes.Register)
				{
					res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token2=" + "Carbiotic.ir" + "&template=CarbioticRegister");
				}
				if (type == (int)SmsTypes.RecoverPassword)
				{
					res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token2=" + token2 + "&template=CarbioticReset");
				}
				if (res != null && res.IsSuccessStatusCode)
				{
					var result = res.Content.ReadAsStringAsync().Result;

					try
					{
						var datalist = JsonConvert.DeserializeObject<SmsViewModel.RootObject>(result);
					}
					catch (Exception e)
					{
						throw;
					}
					return true;
				}

				return false;
			}
		}
	}
}