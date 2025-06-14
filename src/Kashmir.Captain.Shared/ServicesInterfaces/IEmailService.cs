

namespace Kashmir.Captain.Comman
{
	public interface IEmailService
	{
		Task SendEmailAsync(EmailTemplate emailTemplate);
	}
}