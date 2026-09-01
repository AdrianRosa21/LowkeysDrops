using Microsoft.EntityFrameworkCore;
using LowkeysDrops.API.Data;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Repositories {
    public class ConsultasRepository : IConsultasRepository {
        private readonly LowkeysDropsDbContext _context;
        public ConsultasRepository(LowkeysDropsDbContext context) { _context = context; }
        
        public async Task<IEnumerable<VwCatalogoDisponible>> GetCatalogoDisponibleAsync() {
            return await _context.VwCatalogoDisponibles.AsNoTracking().ToListAsync();
        }
        
        public async Task<IEnumerable<VwPedidosResuman>> GetPedidosResumenAsync() {
            return await _context.VwPedidosResumen.AsNoTracking().ToListAsync();
        }
        
        public async Task<IEnumerable<VwPedidosDisponiblesRepartidor>> GetPedidosDisponiblesRepartidorAsync() {
            return await _context.VwPedidosDisponiblesRepartidors.AsNoTracking().ToListAsync();
        }
        
        public async Task<IEnumerable<VwVentasEntregada>> GetVentasEntregadasAsync() {
            return await _context.VwVentasEntregadas.AsNoTracking().ToListAsync();
        }
        
        public async Task<IEnumerable<VwAuditoriaReciente>> GetAuditoriaRecienteAsync() {
            return await _context.VwAuditoriaRecientes.AsNoTracking().ToListAsync();
        }
    }
}
