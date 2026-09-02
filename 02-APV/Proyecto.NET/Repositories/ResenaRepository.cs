using Microsoft.EntityFrameworkCore;
using LowkeysDrops.API.Data;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Repositories {
    public class ResenaRepository : IResenaRepository {
        private readonly LowkeysDropsDbContext _context;
        public ResenaRepository(LowkeysDropsDbContext context) { _context = context; }
        public async Task<IEnumerable<Resena>> GetByProductoIdAsync(int idProducto) {
            return await _context.Resenas
                .Where(r => r.IdProducto == idProducto)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Resena> AddAsync(Resena resena) {
            _context.Resenas.Add(resena);
            await _context.SaveChangesAsync();
            return resena;
        }
    }
}
