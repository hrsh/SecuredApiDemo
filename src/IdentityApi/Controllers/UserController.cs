using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public UserController(
            AppDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJwtGenerator jwtGenerator)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        [HttpGet("login")]
        public async Task<ActionResult<ApiUser>> Login(
            string email,
            string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound($"Could not find user with email {email}!");

            var result = await _signInManager.PasswordSignInAsync(
                user,
                password,
                true,
                true);

            if (!result.Succeeded)
                return Unauthorized("Invalid email or password!");

            var roles = await _userManager.GetRolesAsync(user);
            var userRoles = new List<ApiRole>();
            foreach (var r in roles)
                userRoles.Add(new ApiRole
                {
                    RoleName = r
                });

            var apiUser = new ApiUser
            {
                Id = user.Id,
                Email = user.Email,
                Fullname = user.Fullname,
                Phone = user.PhoneNumber,
                Username = user.UserName,
                Roles = userRoles,
                Token = string.Empty
            };
            var token = _jwtGenerator.Generator(apiUser);
            apiUser.Token = token;
            return apiUser;
        }

        [HttpGet("token")]
        public async Task<ActionResult<string>> GetToken(string username)
        {
            var token = await _jwtGenerator.Generator(username);
            return token;
        }
    }
}
