
namespace Kashmir.Captain.Comman
{
    public interface IUrlHelperService
    {
        string GenerateUrl(string action, string controller, object? values = null);
    }
}