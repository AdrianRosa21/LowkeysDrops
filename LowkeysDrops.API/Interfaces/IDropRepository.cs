using LowkeysDrops.API.Models.Entities;
namespace LowkeysDrops.API.Interfaces {
    public interface IDropRepository {
        Task<IEnumerable<Drop>> GetAllAsync();
        Task<Drop?> GetByIdAsync(int id);
        Task<Drop> AddAsync(Drop drop);
        Task UpdateAsync(Drop drop);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
