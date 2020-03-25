using System.Threading.Tasks;

namespace ECommerce.Services
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
	}
}