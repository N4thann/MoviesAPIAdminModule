namespace MoviesAPIAdminModule.Domain.SeedWork
{
    /// <summary>
    /// Representa a entidade base para todas as entidades do domínio, fornecendo um identificador único.
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public string Name { get; set; }

        protected BaseEntity() => Id = Guid.NewGuid();
    }
}
