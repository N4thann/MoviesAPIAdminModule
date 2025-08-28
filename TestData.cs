using Domain.Entities;
using Domain.ValueObjects;
using Domain.SeedWork.Core;
using System;

namespace Tests.Common
{
    public static class TestData
    {
        #region === Entidades de Apoio (Diretores e Estúdios) ===

        public static Director ChristopherNolan()
        {
            return new Director(
                name: "Christopher Nolan",
                birthDate: new DateTime(1970, 7, 30),
                country: new Country("United Kingdom", "UK")
            );
        }

        public static Studio WarnerBros()
        {
            return new Studio(
                name: "Warner Bros. Pictures",
                foundationDate: new DateTime(1923, 4, 4),
                country: new Country("United States", "USA")
            );
        }

        public static Studio LegendaryPictures()
        {
            return new Studio(
                name: "Legendary Pictures",
                foundationDate: new DateTime(2000, 1, 1),
                country: new Country("United States", "USA")
            );
        }

        #endregion

        #region === Fábricas de Filmes Completos ===

        public static Result<Movie> CreateInceptionMovie()
        {
            var director = ChristopherNolan();
            var studio = WarnerBros();

            var movieResult = Movie.Create(
                title: "A Origem",
                originalTitle: "Inception",
                synopsis: "Um ladrão que rouba segredos corporativos através do uso da tecnologia de compartilhamento de sonhos recebe a tarefa inversa de plantar uma ideia na mente de um C.E.O.",
                releaseYear: 2010,
                duration: new Duration(148),
                country: new Country("United States", "USA"),
                studio: studio,
                director: director,
                genre: new Genre("Science Fiction", "A sci-fi action thriller."),
                boxOffice: new Money(829900000, "USD"),
                budget: new Money(160000000, "USD")
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
            var director = ChristopherNolan();
            var studio = LegendaryPictures();

            var movieResult = Movie.Create(
                title: "O Cavaleiro das Trevas",
                originalTitle: "The Dark Knight",
                synopsis: "Quando a ameaça conhecida como O Coringa emerge de seu passado misterioso, ele causa estragos e caos nas pessoas de Gotham.",
                releaseYear: 2008,
                duration: new Duration(152),
                country: new Country("United States", "USA"),
                studio: studio,
                director: director,
                genre: new Genre("Action", "A superhero thriller."),
                boxOffice: new Money(1006000000, "USD"),
                budget: new Money(185000000, "USD")
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
