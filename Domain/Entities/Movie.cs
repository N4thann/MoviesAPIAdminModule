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
        private const int MAX_TITLE_LENGTH = 200;
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
            int durationInMinutes,
            Country country,
            Studio studio,
            Director director,
            Genre genre) : this()
        {
            ValidateConstructorInputs(title ,originalTitle, synopsis, releaseYear, durationInMinutes, country, studio, director, genre);

            Name = title.Trim();
            OriginalTitle = string.IsNullOrWhiteSpace(originalTitle) ? title.Trim() : originalTitle.Trim();
            Synopsis = synopsis.Trim();
            ReleaseYear = releaseYear;
            Duration = new Duration(durationInMinutes);
            Country = country;
            Studio = studio;
            Director = director;
            Genre = genre;
            Rating = Rating.CreateEmpty(10);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
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

        // Propriedades de controle
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

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
            int duration,
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

            // Validação da duração
            Validate.Range(duration, MIN_DURATION_MINUTES, MAX_DURATION_MINUTES, nameof(duration));

            // Validação de objetos obrigatórios
            Validate.NotNull(country, nameof(country));
            Validate.NotNull(studio, nameof(studio));
            Validate.NotNull(director, nameof(director));
            Validate.NotNull(genre, nameof(genre));
        }

        private static void ValidateBasicInfoUpdate(string title, string originalTitle, string synopsis)
        {
            Validate.NotNullOrEmpty(title, nameof(title));
            Validate.MaxLength(title, MAX_TITLE_LENGTH, nameof(title));

            if (!string.IsNullOrWhiteSpace(originalTitle))
            {
                Validate.MaxLength(originalTitle, MAX_TITLE_LENGTH, nameof(originalTitle));
            }

            Validate.NotNullOrEmpty(synopsis, nameof(synopsis));
            Validate.MaxLength(synopsis, MAX_SYNOPSIS_LENGTH, nameof(synopsis));
        }

        #endregion

        #region Métodos de Negócio - Informações Básicas

        public void UpdateBasicInfo(string title, string originalTitle, string synopsis)
        {
            ValidateBasicInfoUpdate(title, originalTitle, synopsis);

            Name = title.Trim(); // Atualiza BaseEntity.Name
            OriginalTitle = string.IsNullOrWhiteSpace(originalTitle) ? title.Trim() : originalTitle.Trim();
            Synopsis = synopsis.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateProductionInfo(Studio studio, Money? budget = null, Money? boxOffice = null)
        {
            Validate.NotNull(studio, nameof(studio));

            Studio = studio;
            Budget = budget;
            BoxOffice = boxOffice;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDuration(int durationInMinutes)
        {
            Validate.Range(durationInMinutes, MIN_DURATION_MINUTES, MAX_DURATION_MINUTES, nameof(durationInMinutes));

            Duration = new Duration(durationInMinutes);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateReleaseYear(int releaseYear)
        {
            var maxYear = DateTime.UtcNow.Year + MAX_FUTURE_YEARS;
            Validate.Range(releaseYear, MIN_RELEASE_YEAR, maxYear, nameof(releaseYear));

            ReleaseYear = releaseYear;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Métodos de Negócio - Prêmios

        public void AddAward(Award award)
        {
            Validate.NotNull(award, nameof(award));

            if (_awards.Contains(award))
                throw new InvalidOperationException("Filme já possui este prêmio neste ano");

            _awards.Add(award);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAward(Award award)
        {
            Validate.NotNull(award, nameof(award));

            if (!_awards.Remove(award))
                throw new InvalidOperationException("Prêmio não encontrado para esse filme");

            UpdatedAt = DateTime.UtcNow;
        }

        public bool HasAwardFromInstitution(string institution)
        {
            Validate.NotNullOrEmpty(institution, nameof(institution));
            return _awards.Any(a => a.Institution.Equals(institution, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Métodos de Negócio - Imagens

        public void SetPoster(MovieImage poster)
        {
            Validate.NotNull(poster, nameof(poster));

            if (poster.Type != MovieImage.ImageType.Poster)
                throw new InvalidOperationException("Imagem deve ser do tipo Poster");

            // Remove poster existente
            var existingPoster = Poster;
            if (existingPoster != null)
                _images.Remove(existingPoster);

            // Adiciona novo poster
            _images.Add(poster);
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetThumbnail(MovieImage thumbnail)
        {
            Validate.NotNull(thumbnail, nameof(thumbnail));

            if (thumbnail.Type != MovieImage.ImageType.Thumbnail)
                throw new InvalidOperationException("Imagem deve ser do tipo Thumbnail");

            // Remove thumbnail existente
            var existingThumbnail = Thumbnail;
            if (existingThumbnail != null)
                _images.Remove(existingThumbnail);

            // Adiciona novo thumbnail
            _images.Add(thumbnail);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddGalleryImage(MovieImage galleryImage)
        {
            Validate.NotNull(galleryImage, nameof(galleryImage));

            if (galleryImage.Type != MovieImage.ImageType.Gallery)
                throw new InvalidOperationException("Imagem deve ser do tipo Gallery");

            if (GalleryImagesCount >= MAX_GALLERY_IMAGES)
                throw new InvalidOperationException($"Máximo de {MAX_GALLERY_IMAGES} imagens na galeria permitido");

            _images.Add(galleryImage);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveGalleryImage(MovieImage galleryImage)
        {
            Validate.NotNull(galleryImage, nameof(galleryImage));

            if (galleryImage.Type != MovieImage.ImageType.Gallery)
                throw new InvalidOperationException("Apenas imagens de galeria podem ser removidas por este método");

            _images.Remove(galleryImage);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemovePoster()
        {
            var poster = Poster;
            if (poster != null)
            {
                _images.Remove(poster);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void RemoveThumbnail()
        {
            var thumbnail = Thumbnail;
            if (thumbnail != null)
            {
                _images.Remove(thumbnail);
                UpdatedAt = DateTime.UtcNow;
            }
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

        #region Métodos de Negócio - Status

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
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

        #endregion

        public override string ToString()
        {
            return $"{Name} ({ReleaseYear}) - {Duration.ToString} - {Rating}";
        }
    }
}
