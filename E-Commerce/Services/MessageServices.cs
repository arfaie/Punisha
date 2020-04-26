using ECommerce.Models.Helpers.OptionEnums;
using ECommerce.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ECommerce.Services
{
	// This class is used by the application to send Email and SMS
	// when you turn on two-factor authentication in ASP.NET Identity.
	// For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
	public class AuthMessageSender : IEmailSender, ISmsSender
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{
			// Plug in your email service here to send an email.
			return Task.FromResult(0);
		}

		private const string BaseUrl = "https://api.kavenegar.com/";

		public async Task<bool> SendSmsAsync(string mobile, SmsTypes type, string token, string token2 = null)
		{
			var client = new HttpClient
			{
				BaseAddress = new Uri(BaseUrl)
			};

			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage response = null;

			if (type == SmsTypes.Register)
			{
				response = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token2=" + "Carbiotic.ir" + "&template=CarbioticRegister");
			}
			else if (type == SmsTypes.RecoverPassword)
			{
				response = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token2=" + token2 + "&template=CarbioticReset");
			}
			else if (type == SmsTypes.DoneOrder)
			{
				response = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token10=" + token2 + "&template=DoneOrder");
			}

			if (response != null && response.IsSuccessStatusCode)
			{
				var result = response.Content.ReadAsStringAsync().Result;

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