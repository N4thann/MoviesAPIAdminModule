namespace Domain.SeedWork.Interfaces
{
    /// <summary>
    /// Represents the root entity of an aggregate in a domain-driven design context.
    /// </summary>
    /// <remarks>An aggregate root is responsible for maintaining the integrity of the aggregate and serves as
    /// the entry point for accessing and modifying related entities within the aggregate. All external interactions
    /// with the aggregate should be performed through the aggregate root to ensure consistency.</remarks>
    public interface IAggregateRoot
    {

    }
}
