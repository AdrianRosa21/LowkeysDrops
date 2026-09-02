using LowkeysDrops.API.DTOs;
namespace LowkeysDrops.API.Interfaces {
    public interface IDireccionService {
        Task<IEnumerable<DireccionResponseDto>> GetAllAsync();
        Task<DireccionResponseDto?> GetByIdAsync(int id);
        Task<DireccionResponseDto> CreateAsync(DireccionCreateDto dto);
        Task UpdateAsync(int id, DireccionUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
