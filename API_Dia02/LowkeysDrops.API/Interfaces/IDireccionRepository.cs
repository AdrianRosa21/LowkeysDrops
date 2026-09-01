using LowkeysDrops.API.Models.Entities;
namespace LowkeysDrops.API.Interfaces {
    public interface IDireccionRepository {
        Task<IEnumerable<Direccion>> GetAllAsync();
        Task<Direccion?> GetByIdAsync(int id);
        Task<Direccion> AddAsync(Direccion direccion);
        Task UpdateAsync(Direccion direccion);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
