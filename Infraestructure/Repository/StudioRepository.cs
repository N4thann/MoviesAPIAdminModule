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
    }
}
