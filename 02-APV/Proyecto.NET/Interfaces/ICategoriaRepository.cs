using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categorium>> GetAllAsync();
        Task<Categorium?> GetByIdAsync(int id);
        Task<Categorium> AddAsync(Categorium categoria);
        Task UpdateAsync(Categorium categoria);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
