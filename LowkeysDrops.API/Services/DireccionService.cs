using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Services {
    public class DireccionService : IDireccionService {
        private readonly IDireccionRepository _repository;
        public DireccionService(IDireccionRepository repository) { _repository = repository; }
        
        public async Task<IEnumerable<DireccionResponseDto>> GetAllAsync() {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToResponseDto);
        }
        
        public async Task<DireccionResponseDto?> GetByIdAsync(int id) {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return MapToResponseDto(entity);
        }
        
        public async Task<DireccionResponseDto> CreateAsync(DireccionCreateDto dto) {
            var entity = new Direccion {
                IdUsuario = dto.IdUsuario,
                Tipo = dto.Tipo,
                Departamento = dto.Departamento,
                Municipio = dto.Municipio,
                DireccionTexto = dto.DireccionTexto,
                Referencia = dto.Referencia
            };
            var result = await _repository.AddAsync(entity);
            return MapToResponseDto(result);
        }
        
        public async Task UpdateAsync(int id, DireccionUpdateDto dto) {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Direccion no encontrada.");
            
            entity.Tipo = dto.Tipo;
            entity.Departamento = dto.Departamento;
            entity.Municipio = dto.Municipio;
            entity.DireccionTexto = dto.DireccionTexto;
            entity.Referencia = dto.Referencia;
            
            await _repository.UpdateAsync(entity);
        }
        
        public async Task DeleteAsync(int id) {
            var exists = await _repository.ExistsAsync(id);
            if (!exists) throw new KeyNotFoundException("Direccion no encontrada.");
            await _repository.DeleteAsync(id);
        }
        
        private static DireccionResponseDto MapToResponseDto(Direccion p) {
            return new DireccionResponseDto {
                IdDireccion = p.IdDireccion,
                IdUsuario = p.IdUsuario,
                Tipo = p.Tipo,
                Departamento = p.Departamento,
                Municipio = p.Municipio,
                DireccionTexto = p.DireccionTexto,
                Referencia = p.Referencia,
                FechaRegistro = p.FechaRegistro
            };
        }
    }
}
