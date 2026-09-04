using Microsoft.EntityFrameworkCore;
using LowkeysDrops.API.Data;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Repositories {
    public class DropRepository : IDropRepository {
        private readonly LowkeysDropsDbContext _context;
        public DropRepository(LowkeysDropsDbContext context) { _context = context; }
        public async Task<IEnumerable<Drop>> GetAllAsync() { return await _context.Drops.AsNoTracking().ToListAsync(); }
        public async Task<Drop?> GetByIdAsync(int id) { return await _context.Drops.FindAsync(id); }
        public async Task<Drop> AddAsync(Drop drop) { _context.Drops.Add(drop); await _context.SaveChangesAsync(); return drop; }
        public async Task UpdateAsync(Drop drop) { _context.Entry(drop).State = EntityState.Modified; await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var drop = await _context.Drops.FindAsync(id); if (drop != null) { _context.Drops.Remove(drop); await _context.SaveChangesAsync(); } }
        public async Task<bool> ExistsAsync(int id) { return await _context.Drops.AnyAsync(e => e.IdDrop == id); }
    }
}
