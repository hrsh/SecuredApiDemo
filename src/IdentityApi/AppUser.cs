using Microsoft.AspNetCore.Identity;

namespace IdentityApi
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
    }
}
