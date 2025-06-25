using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Infraestructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
