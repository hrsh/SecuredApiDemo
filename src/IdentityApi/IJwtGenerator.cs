using Shared;
using System.Threading.Tasks;

namespace IdentityApi
{
    public interface IJwtGenerator
    {
        string Generator(ApiUser user);

        Task<string> Generator(string username);
    }
}
