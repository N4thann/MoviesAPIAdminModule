using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPIAdminModule.Domain.SeedWork;

namespace Infraestructure.EntitiesConfigurations
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder) 
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            AppendConfig(builder);
        }
        /// <summary>
        ///Essa configuração segue o príncipio do DRY, pegando as configurações comuns
        /// entre as 3 entidades e centralizando aqui. Nas outras entityconfigurations utilizamos
        /// o método AppendConfig, visto que as outras configurações herdam dessa
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void AppendConfig(EntityTypeBuilder<T> builder);
    }
}
