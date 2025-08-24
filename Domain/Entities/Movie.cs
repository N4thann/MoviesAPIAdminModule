using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;
using Domain.SeedWork.Validation;
using Domain.ValueObjects;
using MoviesAPIAdminModule.Domain.SeedWork;

namespace Domain.Entities
{
    public class Movie : BaseEntity, IAggregateRoot
    {
        private const int MIN_RELEASE_YEAR = 1888;
        private const int MAX_FUTURE_YEARS = 5;
        private const int MIN_DURATION_MINUTES = 1;
        private const int MAX_DURATION_MINUTES = 600;
        private const int MAX_TITLE_LENGTH = 100;
        private const int MAX_SYNOPSIS_LENGTH = 2000;
        private const int MAX_GALLERY_IMAGES = 4;

        private readonly List<Award> _awards;
        private readonly List<MovieImage> _images;

        protected Movie()
        {
            _awards = new List<Award>();
            _images = new List<MovieImage>();
        }

        public Movie(
            string title,
            string originalTitle,
            string synopsis,
            int releaseYear,
            Duration duration,
            Country country,
            Studio studio,
            Director director,
            Genre genre,
            Money? boxOffice = null,
            Money? budget = null) : this()
        {
            ValidateConstructorInputs(title ,originalTitle, synopsis, releaseYear, duration, 
                country, studio, director, genre);


            Name = title.Trim();
            OriginalTitle = string.IsNullOrWhiteSpace(originalTitle) ? title.Trim() : originalTitle.Trim();
            Synopsis = synopsis.Trim();
            ReleaseYear = releaseYear;
            Duration = duration;
            Country = country;

            Studio = studio;
            StudioId = Studio.Id;

            BoxOffice = boxOffice;
            Budget = budget;

            Director = director;
            DirectorId = Director.Id;

            Genre = genre;
            Rating = Rating.CreateEmpty(10);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Propriedades principais
        public string OriginalTitle { get; private set; }
        public string Synopsis { get; private set; }
        public int ReleaseYear { get; private set; }
        public Duration Duration { get; private set; }
        public Country Country { get; private set; }
        public Studio Studio { get; private set; }
        public Director Director { get; private set; }
        public Genre Genre { get; private set; }
        public Rating Rating { get; private set; }
        public Money? BoxOffice { get; private set; }
        public Money? Budget { get; private set; }

        // propriedades de chave estrangeira (FKs explícitas)
        public Guid DirectorId { get; private set; }
        public Guid StudioId { get; private set; }

        // Propriedades de controle
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public IReadOnlyCollection<Award> Awards => _awards.AsReadOnly();
        public IReadOnlyCollection<MovieImage> Images => _images.AsReadOnly();

        // Propriedades calculadas para imagens
        public MovieImage? Poster => _images.FirstOrDefault(img => img.Type == MovieImage.ImageType.Poster);
        public MovieImage? Thumbnail => _images.FirstOrDefault(img => img.Type == MovieImage.ImageType.Thumbnail);
        public IEnumerable<MovieImage> GalleryImages => _images.Where(img => img.Type == MovieImage.ImageType.Gallery);

        // Outras propriedades calculadas
        public bool HasPoster => Poster != null;
        public bool HasThumbnail => Thumbnail != null;
        public int GalleryImagesCount => GalleryImages.Count();

        #region Métodos de Validação
        private static void ValidateConstructorInputs(
            string title,
            string originalTitle,
            string synopsis,
            int releaseYear,
            Duration duration,
            Country country,
            Studio studio,
            Director director,
            Genre genre)
        {
            // Validação do título
            Validate.NotNullOrEmpty(title, nameof(title));
            Validate.MaxLength(title, MAX_TITLE_LENGTH, nameof(title));

            // Validação do título original
            if (!string.IsNullOrWhiteSpace(originalTitle))
            {
                Validate.MaxLength(originalTitle, MAX_TITLE_LENGTH, nameof(originalTitle));
            }

            // Validação da sinopse
            Validate.NotNullOrEmpty(synopsis, nameof(synopsis));
            Validate.MaxLength(synopsis, MAX_SYNOPSIS_LENGTH, nameof(synopsis));

            // Validação do ano de lançamento
            var maxYear = DateTime.UtcNow.Year + MAX_FUTURE_YEARS;
            Validate.Range(releaseYear, MIN_RELEASE_YEAR, maxYear, nameof(releaseYear));

            // Validação de objetos obrigatórios
            Validate.NotNull(country, nameof(country));
            Validate.NotNull(duration, nameof(duration));
            Validate.NotNull(studio, nameof(studio));
            Validate.NotNull(director, nameof(director));
            Validate.NotNull(genre, nameof(genre));
        }

        private static void ValidateBasicInfoUpdate(string title, string originalTitle, string synopsis, int durationInMinutes, int releaseYear)
        {
            var maxYear = DateTime.UtcNow.Year + MAX_FUTURE_YEARS;

            Validate.NotNullOrEmpty(title, nameof(title));
            Validate.MaxLength(title, MAX_TITLE_LENGTH, nameof(title));

            Validate.Range(releaseYear, MIN_RELEASE_YEAR, maxYear, nameof(releaseYear));

            Validate.Range(durationInMinutes, MIN_DURATION_MINUTES, MAX_DURATION_MINUTES, nameof(durationInMinutes));

            if (!string.IsNullOrWhiteSpace(originalTitle))
            {
                Validate.MaxLength(originalTitle, MAX_TITLE_LENGTH, nameof(originalTitle));
            }

            Validate.NotNullOrEmpty(synopsis, nameof(synopsis));
            Validate.MaxLength(synopsis, MAX_SYNOPSIS_LENGTH, nameof(synopsis));
        }

        #endregion

        #region Métodos de Negócio - Informações Básicas

        public void UpdateBasicInfo(string title, string originalTitle, string synopsis, int durationInMinutes, int releaseYear)
        {
            ValidateBasicInfoUpdate(title, originalTitle, synopsis, durationInMinutes, releaseYear);

            Name = title.Trim(); // Atualiza BaseEntity.Name
            OriginalTitle = string.IsNullOrWhiteSpace(originalTitle) ? title.Trim() : originalTitle.Trim();
            Synopsis = synopsis.Trim();
            Duration = new Duration(durationInMinutes);
            ReleaseYear = releaseYear;
            UpdatedAt = DateTime.UtcNow;     
        }

        public void UpdateProductionInfo(Money budget, Money boxOffice)
        {
            Validate.NotNull(budget, nameof(budget));
            Validate.NotNull(boxOffice, nameof(boxOffice));

            Budget = budget;
            BoxOffice = boxOffice;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateGenreInfo(Genre genre)
        {
            Validate.NotNull(genre, nameof(genre));

            Genre = genre;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDirectorAndStudioInfo(Guid directorId, Guid studioId)
        {
            StudioId = studioId;
            DirectorId = directorId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateCountry(Country country)
        {
            Validate.NotNull(country, nameof(country));
            Country = country;
            UpdatedAt = DateTime.Now;
        }

        #endregion

        #region Métodos de Negócio - Prêmios

        public Result<bool> AddAward(Award award)
        {
            var validation = Validate.NotNull(award, nameof(award));

            if (validation.IsFailure)
                return Result<bool>.AsFailure(validation.Failure!);

            if (_awards.Contains(award))
                return Result<bool>.AsFailure(Failure.Conflict("Filme já possui este prêmio neste ano"));

            _awards.Add(award);
            UpdatedAt = DateTime.UtcNow;

            return Result<bool>.AsSuccess(true);
        }

        public Result<bool> RemoveAward(Award award)
        {
            var validate = Validate.NotNull(award, nameof(award));

            if (validate.IsFailure) 
                return Result<bool>.AsFailure(validate.Failure!);

            if (!_awards.Remove(award))
                return Result<bool>.AsFailure(Failure.Validation("Prêmio não encontrado para este filme."));

            UpdatedAt = DateTime.UtcNow;

            return Result<bool>.AsSuccess(true);
        }
        #endregion

        #region Métodos de Negócio - Imagens

        public Result<bool> SetPoster(MovieImage poster)
        {
            var validate = Validate.NotNull(poster, nameof(poster));

            if (validate.IsFailure)
                return Result<bool>.AsFailure(validate.Failure!);

            if (poster.Type != MovieImage.ImageType.Poster)
                return Result<bool>.AsFailure(Failure.Validation("Imagem deve ser do tipo Poster"));

            var existingPoster = Poster;

            if (existingPoster != null)
                _images.Remove(existingPoster);

            _images.Add(poster);
            UpdatedAt = DateTime.UtcNow;

            return Result<bool>.AsSuccess(true);
        }

        public Result<bool> SetThumbnail(MovieImage thumbnail)
        {
            Validate.NotNull(thumbnail, nameof(thumbnail));

            if (thumbnail.Type != MovieImage.ImageType.Thumbnail)
                return Result<bool>.AsFailure(Failure.Validation("Imagem deve ser do tipo Thumbnail"));

            var existingThumbnail = Thumbnail;
            if (existingThumbnail != null)
                _images.Remove(existingThumbnail);

            _images.Add(thumbnail);
            UpdatedAt = DateTime.UtcNow;

            return Result<bool>.AsSuccess(true);
        }

        public Result<bool> AddGalleryImage(MovieImage galleryImage)
        {
            var validation = Validate.NotNull(galleryImage, nameof(galleryImage));
            if (validation.IsFailure)
                return Result<bool>.AsFailure(validation.Failure!);

            if (galleryImage.Type != MovieImage.ImageType.Gallery)
                return Result<bool>.AsFailure(Failure.Validation("Image must be of type Gallery."));

            if (GalleryImagesCount >= MAX_GALLERY_IMAGES)
                return Result<bool>.AsFailure(Failure.Conflict($"Maximum of {MAX_GALLERY_IMAGES} gallery images allowed."));

            _images.Add(galleryImage);
            UpdatedAt = DateTime.UtcNow;

            return Result<bool>.AsSuccess(true);
        }

        public Result<bool> RemoveImage(Guid imageId)
        {
            var imageToRemove = _images.FirstOrDefault(i => i.Id == imageId);

            if (imageToRemove is null)
                return Result<bool>.AsFailure(Failure.NotFound("Image", imageId));

            _images.Remove(imageToRemove);
            UpdatedAt = DateTime.UtcNow;

            return Result<bool>.AsSuccess(true);
        }

        #endregion

        #region Métodos de Negócio - Sistema de Avaliação (Rating)

        public void ProcessRatingFromCatalog(decimal voteValue)
        {
            // Validação adicional no Admin por segurança
            Validate.Range((int)voteValue, 1, 10, nameof(voteValue));

            Rating = Rating.AddVote(voteValue);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveRating(decimal voteValue)
        {
            Validate.Range((int)voteValue, 1, 10, nameof(voteValue));

            if (!Rating.HasVotes)
                throw new InvalidOperationException("Cannot remove rating from movie with no ratings");

            Rating = Rating.RemoveVote(voteValue);
            UpdatedAt = DateTime.UtcNow;
        }

        public void ResetRatings()
        {
            Rating = Rating.CreateEmpty(10);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRatingDirectly(decimal totalSum, int votesCount)
        {
            Validate.GreaterThan(votesCount, -1, nameof(votesCount));
            Validate.GreaterThan((int)totalSum, -1, nameof(totalSum));

            Rating = new Rating(totalSum, votesCount, 10);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Métodos de Negócio - Regras Calculadas

        public bool IsBlockbuster()
        {
            return BoxOffice?.Amount >= 100_000_000; 
        }

        public bool IsWellRated(decimal threshold = 7.0m)
        {
            Validate.Range((int)threshold, 1, 10, nameof(threshold));
            return Rating.HasVotes && Rating.AverageValue >= threshold;
        }


        public bool IsClassic()
        {
            return DateTime.UtcNow.Year - ReleaseYear >= 30;
        }

        public bool IsRecent()
        {
            return DateTime.UtcNow.Year - ReleaseYear <= 3;
        }

        public bool WasProfitable()
        {
            return Budget != null && BoxOffice != null &&
                   BoxOffice.Currency == Budget.Currency &&
                   BoxOffice.Amount > Budget.Amount;
        }

        public bool IsLongMovie()
        {
            return Duration.Minutes > 150;
        }

        public override string ToString()
        {
            return $"{Name} ({ReleaseYear}) - {Duration.ToString} - {Rating}";
        }
        #endregion
    }
}
