using LowkeysDrops.API.DTOs;
namespace LowkeysDrops.API.Interfaces {
    public interface IDropService {
        Task<IEnumerable<DropResponseDto>> GetAllAsync();
        Task<DropResponseDto?> GetByIdAsync(int id);
        Task<DropResponseDto> CreateAsync(DropCreateDto dto);
        Task UpdateAsync(int id, DropUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
