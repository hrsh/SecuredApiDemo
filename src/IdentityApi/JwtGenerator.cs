using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityApi
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly UserManager<AppUser> _userManager;

        public JwtGenerator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public string Generator(ApiUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Id)
            };

            if (user.Roles.Any())
                foreach (var role in user.Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));

            var key = Encoding.UTF8.GetBytes("this_is_my_$ecret_key");
            var securityKey = new SymmetricSecurityKey(key);
            var creds = new SigningCredentials(
                securityKey, 
                SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public async Task<string> Generator(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return string.Empty;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
                foreach (var role in roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));

            var key = Encoding.UTF8.GetBytes("this_is_my_$ecret_key");
            var securityKey = new SymmetricSecurityKey(key);
            var creds = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
