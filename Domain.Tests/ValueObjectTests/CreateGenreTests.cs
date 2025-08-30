
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjectTests
{
    public class CreateGenreTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnSuccess()
        {
            // Arrange & Act
            var genreResult = Genre.Create("Science Fiction", "Films featuring futuristic or technological themes.");

            // Assert
            genreResult.IsSuccess.Should().BeTrue();
            genreResult.Success.Should().NotBeNull();
            genreResult.Success.Name.Should().Be("Science Fiction");
            genreResult.Success.Description.Should().Be("Films featuring futuristic or technological themes.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_WithEmptyDescription_ShouldReturnFailure(string emptyDescription)
        {
            // Arrange & Act
            var genreResult = Genre.Create("Horror", emptyDescription);

            // Assert
            genreResult.IsFailure.Should().BeTrue();
            genreResult.Failure.Code.Should().Be(400);
            genreResult.Failure.Message.Should().Contain(emptyDescription);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_WithEmptyName_ShouldReturnFailure(string invalidName)
        {
            // Arrange & Act
            var genreResult = Genre.Create(invalidName, "A valid description.");

            // Assert
            genreResult.IsFailure.Should().BeTrue();
            genreResult.Failure.Code.Should().Be(400);
            genreResult.Failure.Message.Should().Contain(invalidName);
        }

        [Fact]
        public void Create_WithNameTooLong_ShouldReturnFailure()
        {
            // Arrange
            var longName = new string('A', 51); // Max é 50

            // Act
            var genreResult = Genre.Create(longName, "A valid description.");

            // Assert
            genreResult.IsFailure.Should().BeTrue();
            genreResult.Failure.Message.Should().Contain("name");
        }

        [Fact]
        public void Create_WithDescriptionTooLong_ShouldReturnFailure()
        {
            // Arrange
            var longDescription = new string('B', 501); // Max é 500

            // Act
            var genreResult = Genre.Create("Thriller", longDescription);

            // Assert
            genreResult.IsFailure.Should().BeTrue();
            genreResult.Failure.Message.Should().Contain("description");
        }

        [Fact]
        public void TwoGenresWithSameNameInDifferentCases_ShouldBeEqual()
        {
            // Arrange
            var genre1 = Genre.Create("Action", "Description A").Success!;
            var genre2 = Genre.Create("action", "Description B").Success!;

            // Act & Assert
            genre1.Should().Be(genre2);
            (genre1 == genre2).Should().BeTrue();
        }

        [Fact]
        public void TwoGenresWithSameNameAndDifferentDescriptions_ShouldBeEqual()
        {
            // Arrange
            var genre1 = Genre.Create("Drama", "A serious story").Success!;
            var genre2 = Genre.Create("Drama", "Character-driven stories").Success!;

            // Act & Assert
            genre1.Should().Be(genre2);
        }

        [Fact]
        public void TwoGenresWithDifferentNames_ShouldNotBeEqual()
        {
            // Arrange
            var genre1 = Genre.Create("Comedy", "Funny stuff").Success!;
            var genre2 = Genre.Create("Horror", "Scary stuff").Success!;

            // Act & Assert
            genre1.Should().NotBe(genre2);
            (genre1 != genre2).Should().BeTrue();
        }

        [Fact]
        public void ToString_ShouldReturnCorrectlyFormattedString()
        {
            // Arrange
            var genre = Genre.Create("Fantasy", "Magical elements").Success!;
            var expectedString = "Name: Fantasy / Description: Magical elements";

            // Act
            var resultString = genre.ToString();

            // Assert
            resultString.Should().Be(expectedString);
        }
    }
}
