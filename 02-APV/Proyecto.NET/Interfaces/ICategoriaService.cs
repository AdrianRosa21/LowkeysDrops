using LowkeysDrops.API.DTOs;
namespace LowkeysDrops.API.Interfaces {
    public interface ICategoriaService {
        Task<IEnumerable<CategoriaResponseDto>> GetAllAsync();
        Task<CategoriaResponseDto?> GetByIdAsync(int id);
        Task<CategoriaResponseDto> CreateAsync(CategoriaCreateDto dto);
        Task UpdateAsync(int id, CategoriaUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
