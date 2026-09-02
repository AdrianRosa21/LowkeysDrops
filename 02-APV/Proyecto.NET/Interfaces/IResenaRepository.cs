using LowkeysDrops.API.Models.Entities;
namespace LowkeysDrops.API.Interfaces {
    public interface IResenaRepository {
        Task<IEnumerable<Resena>> GetByProductoIdAsync(int idProducto);
        Task<Resena> AddAsync(Resena resena);
    }
}
