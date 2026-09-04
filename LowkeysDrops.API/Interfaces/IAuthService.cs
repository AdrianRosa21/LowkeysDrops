using LowkeysDrops.API.DTOs;

namespace LowkeysDrops.API.Interfaces
{
    public interface IAuthService
    {
        Task<UsuarioResponseDto> RegistrarClienteAsync(RegistroClienteDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<UsuarioResponseDto> ObtenerUsuarioActualAsync(int idUsuario);
        Task<UsuarioResponseDto> ActualizarUsuarioAsync(int idUsuario, ActualizarClienteDto dto);
    }
}
