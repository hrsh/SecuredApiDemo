using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApp
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _client;

        public async Task<string> GetToken()
        {
            var t = await GetTokenFromDb();
            if (string.IsNullOrWhiteSpace(t))
                t = await GetTokenFromService("", "");
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

        private async Task<string> GetTokenFromDb()
        {
            var appToken = await _context
                .Tokens
                .FirstOrDefaultAsync(a => a.Username == "");
            if (appToken is null)
                return string.Empty;
            if (DateTime.Now > appToken.ExpirationDate)
                return string.Empty;

            return appToken.Token;
        }

        private async Task<string> GetTokenFromService(
            string email,
            string password)
        {
            var t = await _client.GetStringAsync(
                $"api/v1/user/login?email={email}&password={password}");

            var user = JsonConvert.DeserializeObject<ApiUser>(t);

            var appToken = new AppToken
            {
                Email = user.Email,
                ExpirationDate = DateTime.Now.AddMinutes(59),
                Password = password,
                Token = user.Token,
                Username = user.Username
            };

            var currentAppToken = await _context
                .Tokens
                .FirstOrDefaultAsync(a => a.Username == user.Username);
            if (currentAppToken is null)
            {
                _context.Tokens.Add(appToken);
                await _context.SaveChangesAsync();
            }
            else
            {
                currentAppToken.Token = user.Token;
                currentAppToken.ExpirationDate = DateTime.Now.AddMinutes(59);

                _context.Tokens.Update(currentAppToken);
                await _context.SaveChangesAsync();
            }

            return user.Token;
        }
    }
}
