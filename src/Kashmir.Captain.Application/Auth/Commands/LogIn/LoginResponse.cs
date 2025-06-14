namespace Kashmir.Captain.Application.Auth
{
    public record LoginResponse(bool IsSuccess, string Message, string Token, string UserId);
}