using System;

namespace WebApp
{
    public class AppToken
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
