using LowkeysDrops.API.DTOs;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Services {
    public class ProductoService : IProductoService {
        private readonly IProductoRepository _repository;
        public ProductoService(IProductoRepository repository) { _repository = repository; }
        
        public async Task<IEnumerable<ProductoResponseDto>> GetAllAsync() {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToResponseDto);
        }
        
        public async Task<ProductoResponseDto?> GetByIdAsync(int id) {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return MapToResponseDto(entity);
        }
        
        public async Task<ProductoResponseDto> CreateAsync(ProductoCreateDto dto) {
            var entity = new Producto {
                IdDrop = dto.IdDrop,
                IdCategoria = dto.IdCategoria,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Talla = dto.Talla,
                Precio = dto.Precio,
                ImagenUrl = dto.ImagenUrl,
                EsUnico = dto.EsUnico,
                Stock = dto.Stock
            };
            var result = await _repository.AddAsync(entity);
            return MapToResponseDto(result);
        }
        
        public async Task UpdateAsync(int id, ProductoUpdateDto dto) {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Producto no encontrado.");
            
            entity.Nombre = dto.Nombre;
            entity.Descripcion = dto.Descripcion;
            entity.Talla = dto.Talla;
            entity.Precio = dto.Precio;
            entity.ImagenUrl = dto.ImagenUrl;
            entity.EsUnico = dto.EsUnico;
            entity.Stock = dto.Stock;
            entity.Estado = dto.Estado;
            
            await _repository.UpdateAsync(entity);
        }
        
        public async Task DeleteAsync(int id) {
            var exists = await _repository.ExistsAsync(id);
            if (!exists) throw new KeyNotFoundException("Producto no encontrado.");
            await _repository.DeleteAsync(id);
        }
        
        private static ProductoResponseDto MapToResponseDto(Producto p) {
            return new ProductoResponseDto {
                IdProducto = p.IdProducto,
                IdDrop = p.IdDrop,
                IdCategoria = p.IdCategoria,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Talla = p.Talla,
                Precio = p.Precio,
                ImagenUrl = p.ImagenUrl,
                EsUnico = p.EsUnico,
                Stock = p.Stock,
                Estado = p.Estado,
                FechaRegistro = p.FechaRegistro
            };
        }
    }
}
