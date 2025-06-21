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
    }
}
