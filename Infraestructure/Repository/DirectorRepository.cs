using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class DirectorRepository
    {
        private readonly DbContext _context;

        public DirectorRepository(DbContext context) => _context = context;

        public async Task AddAsync(Director director)
        {
            
        }
    }
}
