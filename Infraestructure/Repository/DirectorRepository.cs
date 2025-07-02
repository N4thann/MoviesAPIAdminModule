using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repository
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly ApplicationDbContext _context;

        public DirectorRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(Director director)
        {
            await _context.Directors.AddAsync(director);
        }

        public async Task<Director?> GetByIdAsync(Guid id)
        {
            return await _context.Directors.FindAsync(id);
        }

        public async Task<IEnumerable<Director>> GetAllAsync()
        {
            return await _context.Directors.AsNoTracking().ToListAsync();
        }
    }
}
