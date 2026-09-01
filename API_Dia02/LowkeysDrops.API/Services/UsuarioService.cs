using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace LowkeysDrops.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<UsuarioResponseDto> CrearRepartidorAsync(RegistroRepartidorDto dto)
        {
            var existente = await _usuarioRepository.ObtenerPorCorreoAsync(dto.Correo);
            if (existente != null)
                throw new Exception("CONFLICT:El correo ya está registrado.");

            var nuevoUsuario = new Usuario
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Rol = "REPARTIDOR",
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            nuevoUsuario.ContrasenaHash = _passwordHasher.HashPassword(nuevoUsuario, dto.Contrasena);

            var creado = await _usuarioRepository.CrearUsuarioAsync(nuevoUsuario);
            return MapearAUsuarioResponseDto(creado);
        }

        public async Task<IEnumerable<UsuarioResponseDto>> ObtenerTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObtenerTodosAsync();
            return usuarios.Select(MapearAUsuarioResponseDto);
        }

        public async Task<UsuarioResponseDto> ObtenerPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) throw new KeyNotFoundException("Usuario no encontrado.");
            return MapearAUsuarioResponseDto(usuario);
        }

        public async Task CambiarEstadoAsync(int id, CambiarEstadoUsuarioDto dto)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null) throw new KeyNotFoundException("Usuario no encontrado.");
            
            usuario.Estado = dto.Activo;
            await _usuarioRepository.ActualizarAsync(usuario);
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
