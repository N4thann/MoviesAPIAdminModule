using Bogus;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.ValueObjects;

namespace Tests.Shared
{
    /// <summary>
    /// Provides factory methods for generating test data objects with randomized, valid values for domain entities such
    /// as Country, Director, Studio, Genre, and Movie.
    /// </summary>
    /// <remarks>This class is intended for use in unit tests or development scenarios where realistic but
    /// randomly generated data is required. All generated objects use the "pt_BR" locale for data such as names and
    /// addresses. The methods return domain-specific result types, which may indicate validation errors if the
    /// generated data does not meet domain constraints.</remarks>
    public class TestDataBogusFactory
    {
        private static readonly Faker _faker = new Faker("pt_BR");

        /// <summary>
        /// Creates a bogus Country with random valid data.
        /// </summary>
        /// <returns></returns>
        public static Result<Country> CreateCountry()
        {
            return Country.Create(
                name: _faker.Address.Country(),
                code: _faker.Address.CountryCode()
            );
        }

        /// <summary>
        /// Creates a bogus Director with random valid data.
        /// </summary>
        /// <returns></returns>
        public static Result<Director> CreateDirector()
        {
            var country = CreateCountry().Success!;

            return Director.Create(
                name: _faker.Name.FullName(),
                birthDate: _faker.Date.Past(70, new DateTime(1990, 1, 1)),
                country: country
            );
        }

        /// <summary>
        /// Creates a bogus Studio with random valid data.
        /// </summary>
        /// <returns></returns>
        public static Result<Studio> CreateStudio()
        {
            var country = CreateCountry().Success!;

            return Studio.Create(
                name: _faker.Company.CompanyName(),
                foundationDate: _faker.Date.Past(80),
                country: country
                );
        }

        /// <summary>
        /// Creates a bogus Genre with random valid data.
        /// </summary>
        /// <returns></returns>
        public static Result<Genre> CreateGenre()
        {
            return Genre.Create(
                name: _faker.Music.Genre(),// Bogus dont have a specific method for movie genres so we use music genres
                description: _faker.Lorem.Sentence(10)
            );
        }

        /// <summary>
        /// Creates a bogus Movie with random valid data.
        /// </summary>
        public static Result<Movie> CreateBogusMovie()
        {
            var director = CreateDirector().Success!;
            var studio = CreateStudio().Success!;
            var country = CreateCountry().Success!;
            var genre = CreateGenre().Success!;

            var durationResult = Duration.Create(_faker.Random.Int(90, 180));
            var boxOfficeResult = Money.Create(_faker.Random.Decimal(10_000_000, 500_000_000), "USD");
            var budgetResult = Money.Create(_faker.Random.Decimal(1_000_000, 250_000_000), "USD");

            var title = _faker.Lorem.Sentence(3, 1);
            var originalTitle = _faker.Lorem.Sentence(3, 1);

            return Movie.Create(
                title: title,
                originalTitle: originalTitle,
                synopsis: _faker.Lorem.Paragraph(),
                releaseYear: _faker.Random.Int(1980, 2024),
                duration: durationResult.Success!,
                country: country,
                studio: studio,
                director: director,
                genre: genre,
                boxOffice: boxOfficeResult.Success,
                budget: budgetResult.Success
            );
        }
    }
}
