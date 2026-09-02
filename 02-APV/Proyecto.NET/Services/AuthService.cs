using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LowkeysDrops.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<UsuarioResponseDto> RegistrarClienteAsync(RegistroClienteDto dto)
        {
            var existente = await _usuarioRepository.ObtenerPorCorreoAsync(dto.Correo);
            if (existente != null)
                throw new Exception("CONFLICT:El correo ya está registrado.");

            var nuevoUsuario = new Usuario
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Dui = dto.Dui,
                Rol = "CLIENTE",
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            nuevoUsuario.ContrasenaHash = _passwordHasher.HashPassword(nuevoUsuario, dto.Contrasena);

            var creado = await _usuarioRepository.CrearUsuarioAsync(nuevoUsuario);
            return MapearAUsuarioResponseDto(creado);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(dto.Correo);
            if (usuario == null || !usuario.Estado)
                throw new UnauthorizedAccessException("Credenciales incorrectas o usuario inactivo.");

            var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.ContrasenaHash, dto.Contrasena);
            if (result == PasswordVerificationResult.Failed)
            {
                if (usuario.ContrasenaHash.StartsWith("HASH_DEMO_"))
                {
                   throw new UnauthorizedAccessException("El usuario utiliza credenciales de demostración no aptas para inicio de sesión real.");
                }
                throw new UnauthorizedAccessException("Credenciales incorrectas o usuario inactivo.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"] ?? "SuperSecretKeyForDevelopmentOnly1234567890!";
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = MapearAUsuarioResponseDto(usuario)
            };
        }

        public async Task<UsuarioResponseDto> ObtenerUsuarioActualAsync(int idUsuario)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);
            if (usuario == null) throw new KeyNotFoundException("Usuario no encontrado.");
            return MapearAUsuarioResponseDto(usuario);
        }

        private UsuarioResponseDto MapearAUsuarioResponseDto(Usuario u)
        {
            return new UsuarioResponseDto
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Telefono = u.Telefono,
                Dui = u.Dui,
                Rol = u.Rol,
                Estado = u.Estado
            };
        }
    }
}
