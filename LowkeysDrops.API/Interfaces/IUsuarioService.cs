using LowkeysDrops.API.DTOs;

namespace LowkeysDrops.API.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDto> CrearRepartidorAsync(RegistroRepartidorDto dto);
        Task<IEnumerable<UsuarioResponseDto>> ObtenerTodosAsync();
        Task<UsuarioResponseDto> ObtenerPorIdAsync(int id);
        Task CambiarEstadoAsync(int id, CambiarEstadoUsuarioDto dto);
    }
}
