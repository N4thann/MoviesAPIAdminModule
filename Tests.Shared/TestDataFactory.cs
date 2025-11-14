using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.ValueObjects;

namespace Tests.Shared
{
    public static class TestDataFactory
    {
        #region === Entidades de Apoio (Diretores e Estúdios) ===

        public static Result<Director> ChristopherNolan()
        {
            var countryResult = Country.Create("United Kingdom", "UK");
            if (countryResult.IsFailure)
            {
                return Result<Director>.AsFailure(countryResult.Failure!);
            }

            return Director.Create(
                name: "Christopher Nolan",
                birthDate: new DateTime(1970, 7, 30),
                country: countryResult.Success!
            );
        }

        public static Result<Studio> WarnerBros()
        {
            var countryResult = Country.Create("United States", "USA");
            if (countryResult.IsFailure)
            {
                return Result<Studio>.AsFailure(countryResult.Failure!);
            }

            return Studio.Create(
                name: "Warner Bros. Pictures",
                foundationDate: new DateTime(1923, 4, 4),
                country: countryResult.Success!
            );
        }

        public static Result<Studio> LegendaryPictures()
        {
            var countryResult = Country.Create("United States", "USA");
            if (countryResult.IsFailure)
            {
                return Result<Studio>.AsFailure(countryResult.Failure!);
            }

            return Studio.Create(
                name: "Legendary Pictures",
                foundationDate: new DateTime(2000, 1, 1),
                country: countryResult.Success!
            );
        }

        #endregion

        #region === Fábricas de Filmes Completos ===

        public static Result<Movie> CreateInceptionMovie()
        {
            // 1. Obter dependências
            var director = ChristopherNolan().Success;
            var studio = WarnerBros().Success;

            var durationResult = Duration.Create(148);
            var countryResult = Country.Create("United States", "USA");
            var genreResult = Genre.Create("Science Fiction", "A sci-fi action thriller.");
            var boxOfficeResult = Money.Create(829900000, "USD");
            var budgetResult = Money.Create(160000000, "USD");

            var movieResult = Movie.Create(
                title: "A Origem",
                originalTitle: "Inception",
                synopsis: "Um ladrão que rouba segredos corporativos através do uso da tecnologia de compartilhamento de sonhos recebe a tarefa inversa de plantar uma ideia na mente de um C.E.O.",
                releaseYear: 2010,
                duration: durationResult.Success!,
                country: countryResult.Success!,
                studio: studio!,
                director: director!,
                genre: genreResult.Success!,
                boxOffice: boxOfficeResult.Success,
                budget: budgetResult.Success
            );

            if (movieResult.IsFailure) return movieResult;

            var movie = movieResult.Success!;

            var awardResult = Award.Create(AwardCategory.BestOriginalScreenplay, Institution.AcademyAwards, 2011);
            if (awardResult.IsSuccess)
            {
                movie.AddAward(awardResult.Success!);
            }

            var imageUrl = $"https://localhost:7211/StaticFiles/Images/inception-{movie.Id}/inception_2010_poster.jpg";
            var imageResult = MovieImage.Create(imageUrl, "Official movie poster", MovieImage.ImageType.Poster);
            if (imageResult.IsSuccess)
            {
                movie.SetPoster(imageResult.Success!);
            }

            return movieResult; 
        }

        public static Result<Movie> CreateTheDarkKnightMovie()
        {
            var director = ChristopherNolan().Success;
            var studio = LegendaryPictures().Success;

            var durationResult = Duration.Create(152);
            var countryResult = Country.Create("United States", "USA");
            var genreResult = Genre.Create("Action", "A superhero thriller.");
            var boxOfficeResult = Money.Create(1006000000, "USD");
            var budgetResult = Money.Create(185000000, "USD");

            var movieResult = Movie.Create(
                title: "O Cavaleiro das Trevas",
                originalTitle: "The Dark Knight",
                synopsis: "Quando a ameaça conhecida como O Coringa emerge de seu passado misterioso, ele causa estragos e caos nas pessoas de Gotham.",
                releaseYear: 2008,
                duration: durationResult.Success!,
                country: countryResult.Success!,
                studio: studio!,
                director: director!,
                genre: genreResult.Success!,
                boxOffice: boxOfficeResult.Success,
                budget: budgetResult.Success
            );

            if (movieResult.IsFailure) return movieResult;

            var movie = movieResult.Success!;

            var awardResult = Award.Create(AwardCategory.BestSupportingActor, Institution.AcademyAwards, 2009);
            if (awardResult.IsSuccess)
            {
                movie.AddAward(awardResult.Success!);
            }

            var imageUrl = $"https://localhost:7211/StaticFiles/Images/the-dark-knight-{movie.Id}/the-dark-knight_2008_poster.jpg";
            var imageResult = MovieImage.Create(imageUrl, "Official movie poster for The Dark Knight", MovieImage.ImageType.Poster);
            if (imageResult.IsSuccess)
            {
                movie.SetPoster(imageResult.Success!);
            }

            return movieResult;
        }
        #endregion
    }
}
