using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.EntitiesConfigurations
{
    public class StudioConfiguration : BaseEntityConfiguration<Studio>
    {
        public override void Configure(EntityTypeBuilder<Studio> builder)
        {
            base.Configure(builder);

            builder.Property(s => s.History)
                .HasMaxLength(2000);

            builder.Property(s => s.FoundationDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(s => s.IsActive)
                   .IsRequired();

            builder.Property(s => s.CreatedAt)
                   .IsRequired();

            builder.Property(s => s.UpdatedAt)
                   .IsRequired();

            builder.OwnsOne(s => s.Country, countryBuilder =>
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
