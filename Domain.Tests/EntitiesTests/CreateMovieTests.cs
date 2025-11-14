using Bogus;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Tests.Shared;

namespace Domain.Tests.EntitiesTests
{
    public class CreateMovieTests
    {
        private readonly Director _validDirector;
        private readonly Studio _validStudio;
        private readonly Duration _validDuration;
        private readonly Country _validCountry;
        private readonly Genre _validGenre;

        private readonly Faker _faker;
        private readonly string _validTitle;
        private readonly string _validOriginalTitle;
        private readonly string _validSynopsis;
        private readonly int _validReleaseYear;

        public CreateMovieTests()
        {
            _faker = new Faker("pt_BR");

            _validCountry = TestDataBogusFactory.CreateCountry().Success!;
            _validGenre = TestDataBogusFactory.CreateGenre().Success!;
            _validStudio = TestDataBogusFactory.CreateStudio().Success!;
            _validDirector = TestDataBogusFactory.CreateDirector().Success!;
            _validDuration = Duration.Create(120).Success!;

            _validTitle = _faker.Lorem.Sentence(3);
            _validOriginalTitle = _faker.Lorem.Sentence(3);
            _validSynopsis = _faker.Lorem.Sentence(10);
            _validReleaseYear = _faker.Random.Int(1900, DateTime.UtcNow.Year);
        }

        [Fact]
        public void Create_WithValidData_ShouldReturnSuccess()
        {
            // Arrange & Act
            var movieResult = TestDataBogusFactory.CreateBogusMovie();

            // Assert
            movieResult.IsSuccess.Should().BeTrue();
            movieResult.Success.Should().NotBeNull();

            movieResult.Success.Name.Should().NotBeNullOrWhiteSpace();
            movieResult.Success.Synopsis.Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData(null, "cannot be null or empty")]
        [InlineData("", "cannot be null or empty")]
        [InlineData(" ", "cannot be null or empty")]
        public void Create_WithInvalidTitle_ShouldReturnFailure(string invalidTitle, string expectedMessage)
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: invalidTitle, 
                originalTitle: _validOriginalTitle,
                synopsis: _validSynopsis,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain(expectedMessage);
        }

        [Theory]
        [InlineData(null, "cannot be null or empty")]
        [InlineData("", "cannot be null or empty")]
        [InlineData(" ", "cannot be null or empty")]
        public void Create_WithInvalidSynopsis_ShouldReturnFailure(string invalidSynopsis, string expectedMessage)
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: _validOriginalTitle,
                synopsis: invalidSynopsis,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain(expectedMessage);
        }

        [Theory]
        [InlineData("A", "must be at least 10 characters long")]
        [InlineData("Abaaaaaaa", "must be at least 10 characters long")]
        public void Create_WithSynopsisTooShort_ShouldReturnFailure(string shortSynopse, string expectedMessage)
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: _validOriginalTitle,
                synopsis: shortSynopse,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain(expectedMessage);
        }

        [Fact]
        public void Create_WithSynopsisTooLong_ShouldReturnFailure()
        {
            // Arrange
            const int maxSynopsisLength = 2000;
            var longSynopsis = new string('A', maxSynopsisLength + 1);

            // Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: _validOriginalTitle,
                synopsis: longSynopsis,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Code.Should().Be(400);
            movieResult.Failure.Message.Should().Contain("synopsis");
            movieResult.Failure.Message.Should().Contain("2000");
        }

        [Theory]
        [InlineData("A", "must be at least 3 characters long")]
        [InlineData("Ab", "must be at least 3 characters long")]
        public void Create_WithTitleTooShort_ShouldReturnFailure(string shortTitle, string expectedMessage)
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: shortTitle,
                originalTitle: _validOriginalTitle,
                synopsis: _validSynopsis,
                releaseYear: 2023,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain(expectedMessage);
        }

        [Theory]
        [InlineData(null, "cannot be null or empty")]
        [InlineData("", "cannot be null or empty")]
        [InlineData(" ", "cannot be null or empty")]
        public void Create_WithInvalidOriginalTitle_ShouldReturnFailure(string invalidTitle, string expectedMessage)
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: invalidTitle,
                synopsis: _validSynopsis,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain(expectedMessage);
        }

        [Theory]
        [InlineData("A", "must be at least 3 characters long")]
        [InlineData("Ab", "must be at least 3 characters long")]
        public void Create_WithOriginalTitleTooShort_ShouldReturnFailure(string shortTitle, string expectedMessage)
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: shortTitle,
                synopsis: _validSynopsis,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain(expectedMessage);
        }

        [Theory]
        [InlineData(1887)]
        [InlineData(2031)] 
        public void Create_WithReleaseYearOutOfRange_ShouldReturnFailure(int invalidYear)
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: _validOriginalTitle,
                synopsis: _validSynopsis,
                releaseYear: invalidYear,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain("releaseYear");
        }

        [Fact]
        public void Create_WithNullDirector_ShouldReturnFailure()
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: "Filme Válido",
                originalTitle: "Valid Movie",
                synopsis: "Sinopse válida.",
                releaseYear: 2023,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: null,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain("director");
        }

        [Fact]
        public void Create_WithNullStudio_ShouldReturnFailure()
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: _validOriginalTitle,
                synopsis: _validSynopsis,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: _validCountry,
                studio: null,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain("studio");
        }

        [Fact]
        public void Create_WithNullGenre_ShouldReturnFailure()
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: _validOriginalTitle,
                synopsis: _validSynopsis,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: null
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain("genre");
        }

        [Fact]
        public void Create_WithNullCountry_ShouldReturnFailure()
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: _validOriginalTitle,
                synopsis: _validSynopsis,
                releaseYear: _validReleaseYear,
                duration: _validDuration,
                country: null,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain("country");
        }

        [Fact]
        public void Create_WithNullDuration_ShouldReturnFailure()
        {
            // Arrange & Act
            var movieResult = Movie.Create(
                title: _validTitle,
                originalTitle: _validOriginalTitle,
                synopsis: _validSynopsis,
                releaseYear: _validReleaseYear,
                duration: null,
                country: _validCountry,
                studio: _validStudio,
                director: _validDirector,
                genre: _validGenre
            );

            // Assert
            movieResult.IsFailure.Should().BeTrue();
            movieResult.Failure.Message.Should().Contain("duration");
        }
    }
}
