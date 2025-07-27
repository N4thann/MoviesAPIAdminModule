using Domain.SeedWork.Interfaces;
using Infraestructure.Context;

namespace Infraestructure.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context) => _context = context;
    }
}
