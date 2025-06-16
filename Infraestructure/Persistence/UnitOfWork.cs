using Domain.SeedWork.Interfaces;
using Infraestructure.Context;


namespace Infraestructure.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context) => _context = context;

        public async Task Commit(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task Rollback(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
        public void Dispose()
        {
            // Em uma implementação real, o DbContext seria descartado aqui.
            // ((DbContext)_dbContext).Dispose(); // Exemplo se IAdminDbContext fosse um DbContext
            GC.SuppressFinalize(this);
        }
    }
}
