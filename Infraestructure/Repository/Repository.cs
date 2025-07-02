using Domain.SeedWork.Interfaces;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infraestructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context) => _context = context;

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>()
                .AsNoTracking().ToListAsync();
        public IQueryable<T> GetAllQueryable() => _context.Set<T>().AsQueryable();
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) =>
            await _context.Set<T>().AnyAsync(predicate);

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
