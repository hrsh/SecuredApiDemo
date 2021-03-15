using System.Threading.Tasks;

namespace WebApp
{
    public interface ITokenService
    {
        Task<string> GetToken();
    }
}
