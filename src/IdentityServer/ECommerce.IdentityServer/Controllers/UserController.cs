using System.Linq;
using System.Threading.Tasks;
using ECommerce.IdentityServer.DTOs;
using ECommerce.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Shared.DTOs;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.IdentityServer.Controllers
{
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDTO signUpDto)
        {
            var user = new ApplicationUser
            {
                UserName = signUpDto.UserName,
                Email = signUpDto.Email,
                City = signUpDto.City
            };

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded)
                return BadRequest(Response<Shared.DTOs.NoContent>.Fail(result.Errors.Select(item => item.Description).ToList(), 400));

            return NoContent();
        }
    }
}
