using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntitiesConfigurations
{
    public class MovieConfiguration : BaseEntityConfiguration<Movie>
    {
        protected override void AppendConfig(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movie");

            builder.Property(m => m.OriginalTitle)
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(m => m.Synopsis)
                   .HasMaxLength(2000)
                   .IsRequired();

            builder.Property(m => m.ReleaseYear)
                   .IsRequired();

            builder.Property(m => m.CreatedAt)
                   .IsRequired();

            builder.Property(m => m.UpdatedAt)
                   .IsRequired();

            builder.Property(m => m.StudioId)
                   .IsRequired();

            builder.Property(m => m.DirectorId)
                   .IsRequired();

            builder.Ignore(m => m.Poster);
            builder.Ignore(m => m.Thumbnail);
            builder.Ignore(m => m.GalleryImages);
            builder.Ignore(m => m.HasPoster);
            builder.Ignore(m => m.HasThumbnail);
            builder.Ignore(m => m.GalleryImagesCount);

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
                              .HasMaxLength(500)
                              .IsRequired();
            });

            // Owned Collections
            builder.OwnsMany(m => m.Awards, awardBuilder =>
            {
                awardBuilder.ToTable("MovieAwards");

                awardBuilder.WithOwner().HasForeignKey("MovieId");
                awardBuilder.HasKey("Id");
                awardBuilder.Property<int>("Id").ValueGeneratedOnAdd();

                // --- INÍCIO DA MUDANÇA ---

                // Mapeia a propriedade 'Category' (que é um objeto AwardCategory)
                awardBuilder.Property(a => a.Category)
                    .HasColumnName("AwardCategoryId") // Nome da coluna no banco
                    .IsRequired()
                    .HasConversion(
                        // Função para converter o objeto (AwardCategory) para o valor do banco (int)
                        category => category.Id,

                        // Função para converter o valor do banco (int) de volta para o objeto (AwardCategory)
                        id => AwardCategory.FromValue<AwardCategory>(id)
                    );

                // Mapeia a propriedade 'Institution' (que é um objeto Institution)
                awardBuilder.Property(a => a.Institution)
                    .HasColumnName("InstitutionId") // Nome da coluna no banco
                    .IsRequired()
                    .HasConversion(
                        // Objeto -> Banco
                        institution => institution.Id,

                        // Banco -> Objeto
                        id => Institution.FromValue<Institution>(id)
                    );

                // --- FIM DA MUDANÇA ---

                // A propriedade 'Year' continua sendo um tipo primitivo, então o mapeamento é o mesmo
                awardBuilder.Property(a => a.Year).IsRequired();
            });

            builder.OwnsMany(m => m.Images, imageBuilder =>
            {
                imageBuilder.ToTable("MovieImages"); // Define o nome da tabela

                imageBuilder.WithOwner().HasForeignKey("MovieId");
                imageBuilder.HasKey("Id"); // Chave primária da própria linha de imagem
                imageBuilder.Property<Guid>("Id").ValueGeneratedOnAdd();

                imageBuilder.Property(i => i.Url).IsRequired();
                imageBuilder.Property(i => i.AltText).HasMaxLength(200);
                imageBuilder.Property(i => i.Type)
                            .HasConversion<string>() // Boa prática para armazenar enums
                            .IsRequired();
            });

            builder.HasOne(m => m.Director)
                   .WithMany()
                   .HasForeignKey(m => m.DirectorId)
                   .IsRequired();

            builder.HasOne(m => m.Studio)
                   .WithMany()
                   .HasForeignKey(m => m.StudioId)
                   .IsRequired();
        }
    }
}
