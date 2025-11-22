namespace MoviesAPIAdminModule.Domain.SeedWork
{
    /// <summary>
    /// Represents the base entity with common properties.
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public string Name { get; set; }

        protected BaseEntity() => Id = Guid.NewGuid();
    }
}
