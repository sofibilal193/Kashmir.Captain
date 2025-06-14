
namespace Kashmir.Captain.Shared
{
    public record AppSettings
    {
        public SuperAdminCredentials SuperAdminCredentials { get; init; } = new();
        public LoggingSettings Logging { get; init; } = new();
        public ConnectionStringsSettings ConnectionStrings { get; init; } = new();
        public JwtSettings Jwt { get; init; } = new();
        public EmailSettings EmailSettings { get; init; } = new();
        public CryptoSettings Crypto { get; init; } = new();
    }

    public record SuperAdminCredentials
    {
        public string Role { get; init; } = "";
        public string Email { get; init; } = "";
        public string Password { get; init; } = "";
    }

    public record CryptoSettings
    {
        public string Key { get; init; } = "";
        public string KeySize { get; init; } = "";
    }
    public record LoggingSettings
    {
        public LogLevelSettings LogLevel { get; init; } = new();
    }

    public record LogLevelSettings
    {
        public string Default { get; init; } = "";
        public string MicrosoftAspNetCore { get; init; } = "";
    }

    public record ConnectionStringsSettings
    {
        public string IdDbContext { get; init; } = "";
        public string InventoryDbContext { get; init; } = "";
    }

    public record JwtSettings
    {
        public string Key { get; init; } = "";
        public string Issuer { get; init; } = "";
        public string Audience { get; init; } = "";
        public string Subject { get; init; } = "";
    }


    public class EmailSettings
    {
        public string SmtpServer { get; set; } = "";
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = "";
        public string SenderPassword { get; set; } = "";

        public bool EnableSsl { get; set; } = true;
    }
}