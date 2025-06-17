using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntitiesConfigurations
{
    public class DirectorConfiguration : BaseEntityConfiguration<Director>
    {
        public override void Configure(EntityTypeBuilder<Director> builder)
        {
            base.Configure(builder);

            builder.Property(d => d.Biography)
                .HasMaxLength(1000);

            builder.Property(d => d.BirthDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(d => d.Gender)
                .IsRequired()
                .HasConversion<string>();// Armazena o enum como string

            builder.Property(d => d.IsActive)
                .IsRequired();

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(d => d.UpdatedAt)
                .IsRequired();

            builder.OwnsOne(d => d.Country, countryBuilder =>
            {
                countryBuilder.Property(c => c.Name)
                              .HasColumnName("CountryName")
                              .HasMaxLength(100)
                              .IsRequired();

                countryBuilder.Property(c => c.Code)
                              .HasColumnName("CountryCode")
                              .HasMaxLength(3)
                              .IsRequired();
            });
        }
    }
}
