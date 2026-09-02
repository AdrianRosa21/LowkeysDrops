using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Services {
    public class CategoriaService : ICategoriaService {
        private readonly ICategoriaRepository _repository;
        public CategoriaService(ICategoriaRepository repository) { _repository = repository; }
        
        public async Task<IEnumerable<CategoriaResponseDto>> GetAllAsync() {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToResponseDto);
        }
        
        public async Task<CategoriaResponseDto?> GetByIdAsync(int id) {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return MapToResponseDto(entity);
        }
        
        public async Task<CategoriaResponseDto> CreateAsync(CategoriaCreateDto dto) {
            var entity = new Categorium {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };
            var result = await _repository.AddAsync(entity);
            return MapToResponseDto(result);
        }
        
        public async Task UpdateAsync(int id, CategoriaUpdateDto dto) {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Categoria no encontrada.");
            
            entity.Nombre = dto.Nombre;
            entity.Descripcion = dto.Descripcion;
            entity.Estado = dto.Estado;
            
            await _repository.UpdateAsync(entity);
        }
        
        public async Task DeleteAsync(int id) {
            var exists = await _repository.ExistsAsync(id);
            if (!exists) throw new KeyNotFoundException("Categoria no encontrada.");
            await _repository.DeleteAsync(id);
        }
        
        private static CategoriaResponseDto MapToResponseDto(Categorium p) {
            return new CategoriaResponseDto {
                IdCategoria = p.IdCategoria,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Estado = p.Estado
            };
        }
    }
}
