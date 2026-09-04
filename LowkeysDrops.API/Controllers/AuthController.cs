using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LowkeysDrops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroClienteDto dto)
        {
            var usuario = await _authService.RegistrarClienteAsync(dto);
            return StatusCode(201, usuario);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (idClaim == null) return Unauthorized();

            var usuario = await _authService.ObtenerUsuarioActualAsync(int.Parse(idClaim));
            return Ok(usuario);
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] ActualizarClienteDto dto)
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (idClaim == null) return Unauthorized();

            var usuario = await _authService.ActualizarUsuarioAsync(int.Parse(idClaim), dto);
            return Ok(usuario);
        }
    }
}
