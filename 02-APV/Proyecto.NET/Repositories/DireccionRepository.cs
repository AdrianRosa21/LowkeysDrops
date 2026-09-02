using Microsoft.EntityFrameworkCore;
using LowkeysDrops.API.Data;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Repositories {
    public class DireccionRepository : IDireccionRepository {
        private readonly LowkeysDropsDbContext _context;
        public DireccionRepository(LowkeysDropsDbContext context) { _context = context; }
        public async Task<IEnumerable<Direccion>> GetAllAsync() { return await _context.Direccions.AsNoTracking().ToListAsync(); }
        public async Task<Direccion?> GetByIdAsync(int id) { return await _context.Direccions.FindAsync(id); }
        public async Task<Direccion> AddAsync(Direccion direccion) { _context.Direccions.Add(direccion); await _context.SaveChangesAsync(); return direccion; }
        public async Task UpdateAsync(Direccion direccion) { _context.Entry(direccion).State = EntityState.Modified; await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var dir = await _context.Direccions.FindAsync(id); if (dir != null) { _context.Direccions.Remove(dir); await _context.SaveChangesAsync(); } }
        public async Task<bool> ExistsAsync(int id) { return await _context.Direccions.AnyAsync(e => e.IdDireccion == id); }
    }
}
