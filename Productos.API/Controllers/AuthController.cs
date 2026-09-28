using Productos.API.Dtos;
using Productos.API.Models;
using Productos.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Productos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenServices _tokenServices;

        public AuthController(UserManager<ApplicationUser> userManager, TokenServices tokenServices)
        {
            _userManager = userManager;
            _tokenServices = tokenServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user is null) return Unauthorized("Usuario o contraseña incorrectos");

            var resultado = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!resultado) return Unauthorized("Usuario o contraseña incorrectos");

            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenServices.GenerarToken(user, roles.ToList());

            return Ok(new
            {
                token = token,
            });
        }
    }
}
