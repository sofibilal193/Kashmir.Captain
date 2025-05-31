

namespace Kashmir.Captain.Comman
{
    public class EmailTemplate
    {
        public string To { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";

        public void GetEmailRegistrationBody(string? firstName, string? lastName, string? link, string end = "")
        {
            Body = $@"<h2>Welcome {firstName} {lastName} to Kashmir Captain .</h2>
							<p><a href='{link}'>Click here</a> to confirm your email.</p>
							<p>If you did not register for this account, please ignore this email.</p>
							<h3>Thank You For Registration.</h3>";
        }

        public void GetResetPasswordBody(string? link)
        {
            Body = $@"
							<p><a href='{link}'>Click here</a> to reset your Password.</p>
							<p>If you did not register for this account, please ignore this email.</p>";
        }
        public void GetResendUserConfirmationEmailBody(string? link)
        {
            Body = $@"
							<p><a href='{link}'>Click here</a> to Confirm your email.</p>
							<p>If you did not register for this account, please ignore this email.</p>";
        }
    }
}