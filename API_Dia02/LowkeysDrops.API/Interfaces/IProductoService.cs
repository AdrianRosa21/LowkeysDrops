using LowkeysDrops.API.DTOs;
namespace LowkeysDrops.API.Interfaces {
    public interface IProductoService {
        Task<IEnumerable<ProductoResponseDto>> GetAllAsync();
        Task<ProductoResponseDto?> GetByIdAsync(int id);
        Task<ProductoResponseDto> CreateAsync(ProductoCreateDto dto);
        Task UpdateAsync(int id, ProductoUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
