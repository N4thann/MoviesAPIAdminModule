using Application.Commands.Movie;
using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.SeedWork.Validation;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Linq;

namespace Application.UseCases.Movies
{
    public class PatchMovieUseCase : ICommandHandler<PatchMovieCommand, MovieBasicInfoResponse>
    {
        private readonly IRepository<Movie> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PatchMovieUseCase(IRepository<Movie> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieBasicInfoResponse> Handle(PatchMovieCommand command, CancellationToken cancellationToken)
        {
            var movie = await _repository.GetByIdAsync(command.Id);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {command.Id} not found.");

            // Variáveis para acumular os dados do patch, com tipos anuláveis
            string? newTitle = null;
            string? newOriginalTitle = null;
            string? newSynopsis = null;
            int? newDurationInMinutes = null;
            int? newReleaseYear = null;
            Money? newBudget = null;
            Money? newBoxOffice = null;
            Genre? newGenre = null;
            Country? newCountry = null;
            Guid? newDirectorId = null;
            Guid? newStudioId = null;

            // Flags para controlar quais campos foram explicitamente alterados no patch
            bool basicInfoPatched = false;
            bool productionInfoPatched = false;
            bool genrePatched = false;
            bool directorStudioPatched = false;

            foreach (var op in command.PatchDocument.Operations)
            {
                // Normaliza o caminho para comparação sem case-sensitivity
                string normalizedPath = op.path.Trim('/').ToLowerInvariant();

                if (op.OperationType != OperationType.Replace)
                    throw new InvalidOperationException($"Patch operation '{op.OperationType}' for path '{op.path}' is not supported.");

                switch (normalizedPath)
                {
                    case "title":
                        newTitle = op.value?.ToString();
                        basicInfoPatched = true;
                        break;
                    case "originaltitle":
                        newOriginalTitle = op.value?.ToString();
                        basicInfoPatched = true;
                        break;
                    case "synopsis":
                        newSynopsis = op.value?.ToString();
                        basicInfoPatched = true;
                        break;
                    case "duration":
                        if (op.value is int duration)
                        {
                            newDurationInMinutes = duration;
                            basicInfoPatched = true;
                        }
                        else if (op.value is long durationLong)
                        {
                            newDurationInMinutes = (int)durationLong;
                            basicInfoPatched = true;
                        }
                        else if (op.value is string durationString && int.TryParse(durationString, out var parsedDuration))
                        {
                            newDurationInMinutes = parsedDuration;
                            basicInfoPatched = true;
                        }
                        else
                        {
                            throw new ValidationException("durationInMinutes", $"Patch value for 'durationInMinutes' must be a valid integer. Received: {op.value?.GetType().Name} with value: {op.value}");
                        }
                        break;
                    case "releaseyear":
                        if (op.value is int year)
                        {
                            newReleaseYear = year;
                            basicInfoPatched = true;
                        }
                        else if (op.value is string yearString && int.TryParse(yearString, out var parsedYear))
                        {
                            newReleaseYear = parsedYear;
                            basicInfoPatched = true;
                        }
                        else
                        {
                            throw new ValidationException("releaseYear", "Patch value for 'releaseYear' must be a valid integer.");
                        }
                        break;
                    case "budget":
                        if (op.value is JObject budgetJObject)
                        {
                            decimal? amount = budgetJObject["amount"]?.ToObject<decimal>();
                            string? currency = budgetJObject["currency"]?.ToString();
                            if (!amount.HasValue || string.IsNullOrEmpty(currency))
                            {
                                throw new ValidationException("budget", "For budget update, 'amount' and 'currency' are required in the budget object within the patch.");
                            }
                            newBudget = new Money(amount.Value, currency);
                            productionInfoPatched = true;
                        }
                        else
                        {
                            throw new ValidationException("budget", "Patch value for 'budget' must be a JSON object with 'amount' and 'currency'.");
                        }
                        break;
                    case "boxoffice":
                        if (op.value is JObject boxOfficeJObject)
                        {
                            decimal? amount = boxOfficeJObject["amount"]?.ToObject<decimal>();
                            string? currency = boxOfficeJObject["currency"]?.ToString();
                            if (!amount.HasValue || string.IsNullOrEmpty(currency))
                            {
                                throw new ValidationException("boxOffice", "For boxOffice update, 'amount' and 'currency' are required in the boxOffice object within the patch.");
                            }
                            newBoxOffice = new Money(amount.Value, currency);
                            productionInfoPatched = true;
                        }
                        else
                        {
                            throw new ValidationException("boxOffice", "Patch value for 'boxOffice' must be a JSON object with 'amount' and 'currency'.");
                        }
                        break;
                    case "genre":
                        if (op.value is JObject genreJObject)
                        {
                            string? genreName = genreJObject["name"]?.ToString();
                            string? genreDescription = genreJObject["description"]?.ToString();

                            if (string.IsNullOrEmpty(genreName) || string.IsNullOrEmpty(genreDescription))
                            {
                                throw new ValidationException("genre", "For genre update, 'name' and 'description' is required in the genre object within the patch.");
                            }
                            newGenre = new Genre(genreName, genreDescription);
                            genrePatched = true;
                        }
                        else
                        {
                            throw new ValidationException("genre", "Patch value for 'genre' must be a JSON object with 'name'.");
                        }
                        break;
                    case "country":
                        if (op.value is JObject countryJObject)
                        {
                            string? countryName = countryJObject["name"]?.ToString();
                            string? countryCode = countryJObject["code"]?.ToString();
                            if (string.IsNullOrEmpty(countryName) || string.IsNullOrEmpty(countryCode))
                            {
                                throw new ValidationException("country", "For country update, 'name' and 'code' are required in the country object within the patch.");
                            }
                            newCountry = new Country(countryName, countryCode);
                        }
                        else
                        {
                            throw new ValidationException("country", "Patch value for 'country' must be a JSON object with 'name' and 'code'.");
                        }
                        break;
                    case "directorid":
                        if (op.value is string directorIdString && Guid.TryParse(directorIdString, out var parsedDirectorId))
                        {
                            newDirectorId = parsedDirectorId;
                            directorStudioPatched = true;
                        }
                        else if (op.value is Guid directorIdGuid)
                        {
                            newDirectorId = directorIdGuid;
                            directorStudioPatched = true;
                        }
                        else
                        {
                            throw new ValidationException("directorId", "Patch value for 'directorId' must be a valid Guid.");
                        }
                        break;
                    case "studioid":
                        if (op.value is string studioIdString && Guid.TryParse(studioIdString, out var parsedStudioId))
                        {
                            newStudioId = parsedStudioId;
                            directorStudioPatched = true;
                        }
                        else if (op.value is Guid studioIdGuid)
                        {
                            newStudioId = studioIdGuid;
                            directorStudioPatched = true;
                        }
                        else
                        {
                            throw new ValidationException("studioId", "Patch value for 'studioId' must be a valid Guid.");
                        }
                        break;
                    default:
                        throw new InvalidOperationException($"Patch operation for path '{op.path}' is not supported.");
                }
            }

            if (basicInfoPatched)
            {
                movie.UpdateBasicInfo(
                    title: newTitle ?? movie.Name, // Name é herdado de BaseEntity e representa o título
                    originalTitle: newOriginalTitle ?? movie.OriginalTitle,
                    synopsis: newSynopsis ?? movie.Synopsis,
                    durationInMinutes: newDurationInMinutes ?? movie.Duration.Minutes,
                    releaseYear: newReleaseYear ?? movie.ReleaseYear
                );
            }

            if (newCountry != null)
                movie.UpdateCountry(newCountry);

            // Lógica para chamar UpdateProductionInfo
            // Ambos budget e boxOffice devem estar presentes para chamar este método
            if (productionInfoPatched)
            {
                var budgetToUse = newBudget ?? movie.Budget;
                var boxOfficeToUse = newBoxOffice ?? movie.BoxOffice;
                movie.UpdateProductionInfo(budgetToUse, boxOfficeToUse);
            }

            if (genrePatched && newGenre != null)
                movie.UpdateGenreInfo(newGenre);

            if (directorStudioPatched)
            {
                var directorIdToUse = newDirectorId ?? movie.DirectorId;
                var studioIdToUse = newStudioId ?? movie.StudioId;
                movie.UpdateDirectorAndStudioInfo(directorIdToUse, studioIdToUse);
            }

            await _unitOfWork.Commit(cancellationToken);

            var response = movie.ToMovieDTO();
            return response;
        }
    }
}
