
using Kashmir.Captain.Domain;

namespace Kashmir.Captain.Application.Services
{
	public interface IEmailService
	{
		Task SendEmailAsync(EmailTemplate emailTemplate);
	}
}