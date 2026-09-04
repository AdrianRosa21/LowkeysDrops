using LowkeysDrops.API.DTOs;
namespace LowkeysDrops.API.Interfaces {
    public interface IResenaService {
        Task<IEnumerable<ResenaResponseDto>> GetByProductoIdAsync(int idProducto);
        Task<ResenaResponseDto> CreateAsync(int idProducto, ResenaCreateDto dto);
    }
}
