
namespace Kashmir.Captain.Application.Services
{
    public interface IUrlHelperService
    {
        string GenerateUrl(string action, string controller, object? values = null);
    }
}