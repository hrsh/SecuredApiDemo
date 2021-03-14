using Shared;

namespace IdentityApi
{
    public interface IJwtGenerator
    {
        string Generator(ApiUser user);
    }
}
