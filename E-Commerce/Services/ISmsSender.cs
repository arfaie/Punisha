using ECommerce.Models.Helpers.OptionEnums;
using System.Threading.Tasks;

namespace ECommerce.Services
{
	public interface ISmsSender
	{
		Task<bool> SendSmsAsync(string mobile, SmsTypes type, string token, string token2 = null);
	}
}