using Kashmir.Captain.Application.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Kashmir.Captain.Infrastructure.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;
		private readonly SmtpClient _smtpClient;

		public EmailService(IOptions<EmailSettings> emailSettings, SmtpClient smtpClient)
		{
			_emailSettings = emailSettings.Value;
			_smtpClient = smtpClient;
		}
		public async Task SendEmailAsync(Domain.EmailTemplate emailTemplate)
		{
			try
			{
				var mailMessage = new MailMessage(_emailSettings.SenderEmail, emailTemplate.To)
				{
					Sender = new MailAddress(_emailSettings.SenderEmail),
					Subject = emailTemplate.Subject,
					Body = emailTemplate.Body,
					IsBodyHtml = true,
				};
				await _smtpClient.SendMailAsync(mailMessage);
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Email sending was canceled.");
			}
			catch (SmtpException ex)
			{
				Console.WriteLine($"SMTP Error: {ex.Message}");
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to send email: {ex.Message}");
				throw;
			}
		}
	}
}