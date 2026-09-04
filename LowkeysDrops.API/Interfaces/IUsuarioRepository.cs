using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorCorreoAsync(string correo);
        Task<Usuario> CrearUsuarioAsync(Usuario usuario);
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
        Task ActualizarAsync(Usuario usuario);
    }
}
