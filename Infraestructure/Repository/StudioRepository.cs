using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repository
{
    public class StudioRepository : IStudioRepository
    {
        private readonly ApplicationDbContext _context;

        public StudioRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(Studio studio)
        {
            await _context.Studios.AddAsync(studio);
        }

        public async Task<Studio?> GetByIdAsync(Guid id)
        {
            return await _context.Studios.FindAsync(id);
        }

        public async Task<IEnumerable<Studio>> GetAllAsync()
        {
            return await _context.Studios.AsNoTracking().ToListAsync();
        }
    }
}
