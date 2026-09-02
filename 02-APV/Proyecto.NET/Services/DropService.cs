using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Services {
    public class DropService : IDropService {
        private readonly IDropRepository _repository;
        public DropService(IDropRepository repository) { _repository = repository; }
        
        public async Task<IEnumerable<DropResponseDto>> GetAllAsync() {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToResponseDto);
        }
        
        public async Task<DropResponseDto?> GetByIdAsync(int id) {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return MapToResponseDto(entity);
        }
        
        public async Task<DropResponseDto> CreateAsync(DropCreateDto dto) {
            var entity = new Drop {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                FechaPublicacion = dto.FechaPublicacion
            };
            var result = await _repository.AddAsync(entity);
            return MapToResponseDto(result);
        }
        
        public async Task UpdateAsync(int id, DropUpdateDto dto) {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Drop no encontrado.");
            
            entity.Nombre = dto.Nombre;
            entity.Descripcion = dto.Descripcion;
            entity.FechaPublicacion = dto.FechaPublicacion;
            entity.Estado = dto.Estado;
            
            await _repository.UpdateAsync(entity);
        }
        
        public async Task DeleteAsync(int id) {
            var exists = await _repository.ExistsAsync(id);
            if (!exists) throw new KeyNotFoundException("Drop no encontrado.");
            await _repository.DeleteAsync(id);
        }
        
        private static DropResponseDto MapToResponseDto(Drop p) {
            return new DropResponseDto {
                IdDrop = p.IdDrop,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                FechaPublicacion = p.FechaPublicacion,
                Estado = p.Estado
            };
        }
    }
}
