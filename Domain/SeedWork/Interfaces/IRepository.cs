using System.Linq.Expressions;

namespace Domain.SeedWork.Interfaces
{
    /// <summary>
    /// Defines a generic contract for a repository that provides data access operations for entities of type T.
    /// Supports querying, adding, and deleting entities, as well as checking for existence and retrieving entities by
    /// identifier.
    /// </summary>
    /// <remarks>This interface abstracts common data access patterns for working with entities in a
    /// persistence store. Implementations typically support asynchronous operations for scalability and may be used in
    /// conjunction with a Unit of Work pattern. The repository does not prescribe how entities are stored or retrieved,
    /// allowing flexibility in underlying data sources (such as databases or in-memory collections).</remarks>
    /// <typeparam name="T">The type of entity managed by the repository. Must be a reference type.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Asynchronously retrieves an entity of type T by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity of type T if found;
        /// otherwise, null.</returns>
        Task<T> GetByIdAsync(Guid id);
        /// <summary>
        /// Asynchronously retrieves all entities of type <typeparamref name="T"/> from the data source.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all
        /// entities of type <typeparamref name="T"/>. If no entities are found, the collection will be empty.</returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Returns a queryable collection of all entities of type T in the data source.
        /// </summary>
        /// <remarks>The returned <see cref="IQueryable{T}"/> allows for deferred execution and further
        /// composition of queries using LINQ operators. Changes to the underlying data source after obtaining the
        /// queryable may affect the results when the query is executed.</remarks>
        /// <returns>An <see cref="IQueryable{T}"/> that can be used to query all entities of type T. The returned query is not
        /// executed until enumerated.</returns>
        IQueryable<T> GetAllQueryable();
        /// <summary>
        /// Asynchronously determines whether any entities match the specified criteria.
        /// </summary>
        /// <param name="predicate">An expression that defines the conditions to test against entities of type <typeparamref name="T"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if at
        /// least one entity matches the criteria; otherwise, <see langword="false"/>.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Adds the specified entity to the collection.
        /// </summary>
        /// <param name="entity">The entity to add to the collection. Cannot be null.</param>
        void Add(T entity);
        /// <summary>
        /// Adds the elements of the specified collection to the current set.
        /// </summary>
        /// <param name="entities">The collection of entities to add. Cannot be null.</param>
        void AddRange(IEnumerable<T> entities);
        /// <summary>
        /// Removes the specified entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to be deleted. Cannot be null.</param>
        void Delete(T entity);
    }
}

