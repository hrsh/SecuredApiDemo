using Microsoft.IdentityModel.Tokens;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace IdentityApi
{
    public class JwtGenerator : IJwtGenerator
    {
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
    }
}
