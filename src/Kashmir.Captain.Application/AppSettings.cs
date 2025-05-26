using Kashmir.Captain.Application;
using Kashmir.Captain.Domain;

namespace ODL.Application.Common
{
    public record AppSettings
    {
        public LoggingSettings Logging { get; init; } = new();
        public ConnectionStringsSettings ConnectionStrings { get; init; } = new();
        public JwtSettings Jwt { get; init; } = new();
        public EmailSettings EmailSettings { get; init; } = new();
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
        public string KcDbContext { get; init; } = "";
        public string KcIdentityDbContext { get; init; } = "";
    }

    public record JwtSettings
    {
        public string Key { get; init; } = "";
        public string Issuer { get; init; } = "";
        public string Audience { get; init; } = "";
        public string Subject { get; init; } = "";
    }

}