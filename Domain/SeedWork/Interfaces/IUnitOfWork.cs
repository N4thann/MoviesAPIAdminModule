namespace Domain.SeedWork.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits all pending changes as a single atomic operation.
        /// </summary>
        /// <param name="cacellationToken">A cancellation token that can be used to cancel the commit operation.</param>
        /// <returns>A task that represents the asynchronous commit operation.</returns>
        Task Commit(CancellationToken cacellationToken);
        /// <summary>
        /// Attempts to revert all changes made during the current transaction asynchronously.
        /// </summary>
        /// <param name="cacellationToken">A cancellation token that can be used to cancel the rollback operation.</param>
        /// <returns>A task that represents the asynchronous rollback operation.</returns>
        Task Rollback(CancellationToken cacellationToken);
    }
}
