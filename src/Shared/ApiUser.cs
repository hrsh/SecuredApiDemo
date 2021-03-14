using System.Collections.Generic;

namespace Shared
{
    public class ApiUser
    {
        public string Id { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Phone { get; set; }

        public List<ApiRole> Roles { get; set; }

        public string Token { get; set; }
    }
}
