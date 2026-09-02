using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Services {
    public class ResenaService : IResenaService {
        private readonly IResenaRepository _repository;
        public ResenaService(IResenaRepository repository) { _repository = repository; }
        
        public async Task<IEnumerable<ResenaResponseDto>> GetByProductoIdAsync(int idProducto) {
            var entities = await _repository.GetByProductoIdAsync(idProducto);
            return entities.Select(MapToResponseDto);
        }
        
        public async Task<ResenaResponseDto> CreateAsync(int idProducto, ResenaCreateDto dto) {
            var entity = new Resena {
                IdProducto = idProducto,
                IdCliente = dto.IdCliente,
                Calificacion = dto.Calificacion,
                Comentario = dto.Comentario
            };
            var result = await _repository.AddAsync(entity);
            return MapToResponseDto(result);
        }
        
        private static ResenaResponseDto MapToResponseDto(Resena r) {
            return new ResenaResponseDto {
                IdResena = r.IdResena,
                IdProducto = r.IdProducto,
                IdCliente = r.IdCliente,
                Calificacion = r.Calificacion,
                Comentario = r.Comentario,
                Fecha = r.Fecha
            };
        }
    }
}
