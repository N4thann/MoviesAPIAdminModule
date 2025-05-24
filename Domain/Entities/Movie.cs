using Domain.SeedWork.Validation;
using Domain.ValueObjects;
using MoviesAPIAdminModule.Domain.SeedWork;

namespace Domain.Entities
{
    public class Movie : BaseEntity
    {
        // Constantes para validação
        private const int MIN_RELEASE_YEAR = 1888; // Primeiro filme da história
        private const int MAX_FUTURE_YEARS = 5;
        private const int MIN_DURATION_MINUTES = 1;
        private const int MAX_DURATION_MINUTES = 600; // 10 horas
        private const int MAX_TITLE_LENGTH = 200;
        private const int MAX_SYNOPSIS_LENGTH = 2000;

        private readonly List<Award> _awards;
        private readonly List<MovieImage> _images;
        private readonly List<Director> _directors;

        protected Movie()
        {
            _awards = new List<Award>();
            _images = new List<MovieImage>();
            _directors = new List<Director>();
        }
        public Movie(
            string title,
            string originalTitle,
            string synopsis,
            int releaseYear,
            int durationInMinutes,
            Country country,
            Studio studio) : this()
        {
            ValidateConstructorInputs(title ,originalTitle, synopsis, releaseYear, durationInMinutes, country, studio);

            Name = title.Trim();
            OriginalTitle = string.IsNullOrWhiteSpace(originalTitle) ? title.Trim() : originalTitle.Trim();
            Synopsis = synopsis.Trim();
            ReleaseYear = releaseYear;
            Duration = new Duration(durationInMinutes);//Verificar se as demais variáveis são atribuídas
            Country = country;
            Studio = studio;
            Rating = Rating.CreateEmpty(10); // Escala de 1-10 para votos dos clientes
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
        public Rating Rating { get; private set; }
        public Money? BoxOffice { get; private set; }
        public Money? Budget { get; private set; }

        // Propriedades de controle
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        // Coleções somente leitura (virtual para EF Core)
        public virtual ICollection<Award> Awards => _awards;
        public virtual ICollection<MovieImage> Images => _images;
        public IReadOnlyCollection<Director> Directors => _directors.AsReadOnly();

        // Propriedades calculadas para imagens
        public MovieImage? Poster => _images.FirstOrDefault(img => img.Type == MovieImage.ImageType.Poster);
        public IEnumerable<MovieImage> GalleryImages => _images.Where(img => img.Type == MovieImage.ImageType.Gallery);
        public MovieImage? Thumbnail => _images.FirstOrDefault(img => img.Type == MovieImage.ImageType.Thumbnail);

        // Outras propriedades calculadas
        public bool HasAwards => _awards.Any();
        public bool HasImages => _images.Any();
        public bool HasPoster => Poster != null;

        #region Métodos de Validação

        private static void ValidateConstructorInputs(
            string title,
            string originalTitle,
            string synopsis,
            int releaseYear,
            int duration,
            Country country,
            Studio studio)
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

        /// <summary>
        /// Atualiza as informações básicas do filme
        /// </summary>
        public void UpdateBasicInfo(string title, string originalTitle, string synopsis)
        {
            ValidateBasicInfoUpdate(title, originalTitle, synopsis);

            Name = title.Trim(); // Atualiza BaseEntity.Name
            OriginalTitle = string.IsNullOrWhiteSpace(originalTitle) ? title.Trim() : originalTitle.Trim();
            Synopsis = synopsis.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Atualiza informações de produção
        /// </summary>
        public void UpdateProductionInfo(Studio studio, Money? budget = null, Money? boxOffice = null)
        {
            Validate.NotNull(studio, nameof(studio));

            Studio = studio;
            Budget = budget;
            BoxOffice = boxOffice;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Atualiza a duração do filme
        /// </summary>
        public void UpdateDuration(int durationInMinutes)
        {
            Validate.Range(durationInMinutes, MIN_DURATION_MINUTES, MAX_DURATION_MINUTES, nameof(durationInMinutes));

            Duration = durationInMinutes;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Atualiza o ano de lançamento
        /// </summary>
        public void UpdateReleaseYear(int releaseYear)
        {
            var maxYear = DateTime.UtcNow.Year + MAX_FUTURE_YEARS;
            Validate.Range(releaseYear, MIN_RELEASE_YEAR, maxYear, nameof(releaseYear));

            ReleaseYear = releaseYear;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Métodos de Negócio - Diretores

        /// <summary>
        /// Adiciona um diretor ao filme
        /// </summary>
        public void AddDirector(Director director)
        {
            Validate.NotNull(director, nameof(director));

            if (_directors.Any(d => d.Id == director.Id))
                throw new InvalidOperationException("Director already exists for this movie");

            _directors.Add(director);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove um diretor do filme
        /// </summary>
        public void RemoveDirector(Guid directorId)
        {
            var director = _directors.FirstOrDefault(d => d.Id == directorId);
            if (director == null)
                throw new InvalidOperationException("Director not found for this movie");

            _directors.Remove(director);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Métodos de Negócio - Prêmios

        /// <summary>
        /// Adiciona um prêmio ao filme
        /// </summary>
        public void AddAward(Award award)
        {
            Validate.NotNull(award, nameof(award));

            if (_awards.Contains(award))
                throw new InvalidOperationException("This award already exists for this movie");

            _awards.Add(award);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove um prêmio do filme
        /// </summary>
        public void RemoveAward(Award award)
        {
            Validate.NotNull(award, nameof(award));

            if (!_awards.Remove(award))
                throw new InvalidOperationException("Award not found for this movie");

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Verifica se o filme possui um tipo específico de prêmio
        /// </summary>
        public bool HasAwardFromInstitution(string institution)
        {
            Validate.NotNullOrEmpty(institution, nameof(institution));
            return _awards.Any(a => a.Institution.Equals(institution, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Métodos de Negócio - Imagens

        /// <summary>
        /// Adiciona uma imagem ao filme
        /// </summary>
        public void AddImage(MovieImage image)
        {
            Validate.NotNull(image, nameof(image));

            // Se é um poster e já existe um, remove o anterior
            if (image.Type == MovieImage.ImageType.Poster && HasPoster)
            {
                var existingPoster = Poster;
                _images.Remove(existingPoster);
            }

            _images.Add(image);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove uma imagem do filme
        /// </summary>
        public void RemoveImage(MovieImage image)
        {
            Validate.NotNull(image, nameof(image));

            if (!_images.Remove(image))
                throw new InvalidOperationException("Image not found for this movie");

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Define o poster do filme
        /// </summary>
        public void SetPoster(string url, string altText = null)
        {
            Validate.NotNullOrEmpty(url, nameof(url));
            var posterImage = MovieImage.CreatePoster(url, altText);
            AddImage(posterImage);
        }

        /// <summary>
        /// Adiciona uma imagem à galeria
        /// </summary>
        public void AddGalleryImage(string url, string altText = null)
        {
            Validate.NotNullOrEmpty(url, nameof(url));
            var galleryImage = MovieImage.CreateGallery(url, altText);
            AddImage(galleryImage);
        }

        /// <summary>
        /// Define a thumbnail do filme
        /// </summary>
        public void SetThumbnail(string url, string altText = null)
        {
            Validate.NotNullOrEmpty(url, nameof(url));
            var thumbnailImage = MovieImage.CreateThumbnail(url, altText);
            AddImage(thumbnailImage);
        }

        #endregion

        #region Métodos de Negócio - Sistema de Avaliação (Rating)

        /// <summary>
        /// Processa um voto recebido do módulo Catalog via evento
        /// Voto deve estar entre 1 e 10 (validado no Catalog)
        /// </summary>
        public void ProcessRatingFromCatalog(decimal voteValue)
        {
            // Validação adicional no Admin por segurança
            Validate.Range((int)voteValue, 1, 10, nameof(voteValue));

            Rating = Rating.AddVote(voteValue);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove uma avaliação específica (para correções administrativas)
        /// </summary>
        public void RemoveRating(decimal voteValue)
        {
            Validate.Range((int)voteValue, 1, 10, nameof(voteValue));

            if (!Rating.HasVotes)
                throw new InvalidOperationException("Cannot remove rating from movie with no ratings");

            Rating = Rating.RemoveVote(voteValue);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Redefine todas as avaliações (para recálculos administrativos)
        /// </summary>
        public void ResetRatings()
        {
            Rating = Rating.CreateEmpty(10);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Atualiza o rating diretamente com valores calculados (para migrações/correções)
        /// </summary>
        public void UpdateRatingDirectly(decimal totalSum, int votesCount)
        {
            Validate.GreaterThan(votesCount, -1, nameof(votesCount));
            Validate.GreaterThan((int)totalSum, -1, nameof(totalSum));

            Rating = new Rating(totalSum, votesCount, 10);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Métodos de Negócio - Status

        /// <summary>
        /// Ativa o filme
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Desativa o filme
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Métodos de Negócio - Regras Calculadas

        /// <summary>
        /// Verifica se o filme pode ser considerado um blockbuster
        /// </summary>
        public bool IsBlockbuster()
        {
            return BoxOffice?.Amount >= 100_000_000; // 100 milhões
        }

        /// <summary>
        /// Verifica se o filme é bem avaliado
        /// </summary>
        public bool IsWellRated(decimal threshold = 7.0m)
        {
            Validate.Range((int)threshold, 1, 10, nameof(threshold));
            return Rating.HasVotes && Rating.AverageValue >= threshold;
        }

        /// <summary>
        /// Verifica se é um filme clássico (mais de 30 anos)
        /// </summary>
        public bool IsClassic()
        {
            return DateTime.UtcNow.Year - ReleaseYear >= 30;
        }

        /// <summary>
        /// Verifica se é um filme recente (últimos 3 anos)
        /// </summary>
        public bool IsRecent()
        {
            return DateTime.UtcNow.Year - ReleaseYear <= 3;
        }

        /// <summary>
        /// Verifica se o filme teve lucro
        /// </summary>
        public bool WasProfitable()
        {
            return Budget != null && BoxOffice != null &&
                   BoxOffice.Currency == Budget.Currency &&
                   BoxOffice.Amount > Budget.Amount;
        }

        /// <summary>
        /// Verifica se é um filme longo (mais de 150 minutos)
        /// </summary>
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
