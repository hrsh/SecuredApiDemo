using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public UserController(
            AppDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

            return new ApiUser
            {
                Id = user.Id,
                Email = user.Email,
                Fullname = user.Fullname,
                Phone = user.PhoneNumber,
                Username = user.UserName,
                Roles = userRoles
            };
        }
    }
}
