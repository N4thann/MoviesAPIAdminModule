using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntitiesConfigurations
{
    public class MovieConfiguration : BaseEntityConfiguration<Movie>
    {
        protected override void AppendConfig(EntityTypeBuilder<Movie> builder)
        {
            base.Configure(builder);

            builder.ToTable("Movie");

            builder.Property(m => m.OriginalTitle)
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(m => m.Synopsis)
                   .HasMaxLength(2000)
                   .IsRequired();

            builder.Property(m => m.ReleaseYear)
                   .IsRequired();

            builder.Property(m => m.IsActive)
                   .IsRequired();

            builder.Property(m => m.CreatedAt)
                   .IsRequired();

            builder.Property(m => m.UpdatedAt)
                   .IsRequired();

            // Owned Types
            builder.OwnsOne(m => m.Duration, durationBuilder =>
            {
                durationBuilder.Property(d => d.Minutes)
                               .HasColumnName("DurationInMinutes")
                               .IsRequired();
            });

            builder.OwnsOne(m => m.Country, countryBuilder =>
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

            builder.OwnsOne(m => m.Rating, ratingBuilder =>
            {
                ratingBuilder.Property(r => r.TotalSum)
                             .HasColumnName("RatingTotalSum")
                             .HasColumnType("decimal(18,2)")
                             .IsRequired();

                ratingBuilder.Property(r => r.VotesCount)
                             .HasColumnName("RatingVotesCount")
                             .IsRequired();

                ratingBuilder.Property(r => r.MaxValue)
                             .HasColumnName("RatingMaxValue")
                             .HasColumnType("decimal(18,2)")
                             .IsRequired();
            });

            builder.OwnsOne(m => m.BoxOffice, moneyBuilder =>
            {
                moneyBuilder.Property(mo => mo.Amount)
                            .HasColumnName("BoxOfficeAmount")
                            .HasColumnType("decimal(18,2)");

                moneyBuilder.Property(mo => mo.Currency)
                            .HasColumnName("BoxOfficeCurrency")
                            .HasMaxLength(3);
            });

            builder.OwnsOne(m => m.Budget, moneyBuilder =>
            {
                moneyBuilder.Property(mo => mo.Amount)
                            .HasColumnName("BudgetAmount")
                            .HasColumnType("decimal(18,2)");

                moneyBuilder.Property(mo => mo.Currency)
                            .HasColumnName("BudgetCurrency")
                            .HasMaxLength(3);
            });

            builder.OwnsOne(m => m.Genre, genreBuilder =>
            {
                genreBuilder.Property(c => c.Name)
                              .HasColumnName("GenreName")
                              .HasMaxLength(50)
                              .IsRequired();

                genreBuilder.Property(c => c.Description)
                              .HasColumnName("GenreDescription")
                              .HasMaxLength(500);
            });

            builder.OwnsMany(m => m.Awards, awardBuilder =>
            {
                awardBuilder.ToTable("MovieAwards"); // Tabela para a coleção de Awards
                awardBuilder.Property(a => a.Name)
                            .HasMaxLength(200)
                            .IsRequired();

                awardBuilder.Property(a => a.Institution)
                            .HasMaxLength(200)
                            .IsRequired();

                awardBuilder.Property(a => a.Year)
                            .IsRequired();

                awardBuilder.HasKey("Id"); // Chave primária interna para a Owned Entity Type
                awardBuilder.Property("Id").ValueGeneratedOnAdd(); // Id será gerado para cada item da coleção
                awardBuilder.WithOwner().HasForeignKey("MovieId"); // Chave estrangeira de volta para Movie
            });

            builder.OwnsMany(m => m.Images, imageBuilder =>
            {
            imageBuilder.ToTable("MovieImages"); // Tabela para a coleção de MovieImages
            imageBuilder.Property(i => i.Url)
                        .IsRequired();
            imageBuilder.Property(i => i.AltText)
                        .HasMaxLength(200);
                imageBuilder.Property(i => i.Type)
                            .HasConversion<string>() // Armazena o enum como string
                            .IsRequired();
            imageBuilder.HasKey("Id"); // Chave primária interna para a Owned Entity Type
            imageBuilder.Property("Id").ValueGeneratedOnAdd(); // Id será gerado para cada item da coleção
            imageBuilder.WithOwner().HasForeignKey("MovieId"); // Chave estrangeira de volta para Movie
        });

            // Relacionamentos com outras entidades (assuming they are separate entities and not owned types)
        builder.HasOne(m => m.Director)
                   .WithMany()
                   .HasForeignKey(m => m.Director.Id) // Supondo que você mapeará a propriedade do Value Object
                   .IsRequired();

        builder.HasOne(m => m.Studio)
                   .WithMany()
                   .HasForeignKey(m => m.Studio.Id)
                   .IsRequired();
        }
    }
}
