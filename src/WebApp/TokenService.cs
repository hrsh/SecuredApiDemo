using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _client;

        public async Task<string> GetToken(string username)
        {
            var t = await GetTokenFromDb(username);
            if (string.IsNullOrWhiteSpace(t))
                t = await GetTokenFromService(username);
            return t;
        }

        public TokenService(
            AppDbContext context,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _client = httpClientFactory.CreateClient("token_api");
            _client.BaseAddress = new Uri("https://localhost:6001");
        }

        private async Task<string> GetTokenFromDb(string username)
        {
            var appToken = await _context
                .Tokens
                .FirstOrDefaultAsync(a => a.Username == username);

            if (appToken is null)
                return string.Empty;

            return appToken.Token;
        }

        private async Task<string> GetTokenFromService(
            string username)
        {
            var t = await _client.GetStringAsync(
                $"api/v1/user/token?username={username}");

            if (string.IsNullOrWhiteSpace(t))
                return string.Empty;

            var currentAppToken = await _context
                .Tokens
                .FirstOrDefaultAsync(a => a.Username == username);

            var appToken = new AppToken
            {
                Token = t,
                Username = username
            };

            if (currentAppToken is null)
            {
                _context.Tokens.Add(appToken);
                await _context.SaveChangesAsync();
            }
            else
            {
                currentAppToken.Token = t;
                _context.Tokens.Update(currentAppToken);
                await _context.SaveChangesAsync();
            }

            return t;
        }
    }
}
