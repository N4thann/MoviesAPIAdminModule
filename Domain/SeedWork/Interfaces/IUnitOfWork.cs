namespace Domain.SeedWork.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken cacellationToken);
        Task Rollback(CancellationToken cacellationToken);
    }
}
