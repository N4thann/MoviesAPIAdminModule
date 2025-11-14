using Bogus;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.ValueObjects;

namespace Tests.Shared
{
    public class TestDataBogusFactory
    {
        private static readonly Faker _faker = new Faker("pt_BR");

        public static Result<Country> CreateCountry()
        {
            return Country.Create(
                name: _faker.Address.Country(),
                code: _faker.Address.CountryCode()
            );
        }

        public static Result<Director> CreateDirector()
        {
            var country = CreateCountry().Success!;

            return Director.Create(
                name: _faker.Name.FullName(),
                birthDate: _faker.Date.Past(70, new DateTime(1990, 1, 1)),
                country: country
            );
        }

        public static Result<Studio> CreateStudio()
        {
            var country = CreateCountry().Success!;

            return Studio.Create(
                name: _faker.Company.CompanyName(),
                foundationDate: _faker.Date.Past(80),
                country: country
                );
        }

        public static Result<Genre> CreateGenre()
        {
            return Genre.Create(
                name: _faker.Music.Genre(),// Bogus não tem um genero de filme, então usamos música
                description: _faker.Lorem.Sentence(10)
            );
        }

        /// <summary>
        /// Cria uma entidade Movie completa com dados aleatórios válidos.
        /// Ideal para testes que precisam de "qualquer filme", sem se 
        /// importar com os dados específicos.
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
