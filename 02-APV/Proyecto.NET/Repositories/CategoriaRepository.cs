using Microsoft.EntityFrameworkCore;
using LowkeysDrops.API.Data;
using LowkeysDrops.API.Interfaces;
using LowkeysDrops.API.Models.Entities;

namespace LowkeysDrops.API.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly LowkeysDropsDbContext _context;

        public CategoriaRepository(LowkeysDropsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categorium>> GetAllAsync()
        {
            return await _context.Categoria.AsNoTracking().ToListAsync();
        }

        public async Task<Categorium?> GetByIdAsync(int id)
        {
            return await _context.Categoria.FindAsync(id);
        }

        public async Task<Categorium> AddAsync(Categorium categoria)
        {
            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task UpdateAsync(Categorium categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria != null)
            {
                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categoria.AnyAsync(e => e.IdCategoria == id);
        }
    }
}
