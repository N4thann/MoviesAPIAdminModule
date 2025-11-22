
using Bogus;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjectTests
{
    public class CreateGenreTests
    {
        private readonly Faker _faker;

        public CreateGenreTests() => _faker = new Faker("pt_BR");           

        [Fact]
        public void Create_WithValidData_ShouldReturnSuccess()
        {
            var validName = _faker.Lorem.Sentence(3);
            var validDescription = _faker.Lorem.Sentence(10);
            // Arrange & Act
            var genreResult = Genre.Create(validName, validDescription);

            // Assert
            genreResult.IsSuccess.Should().BeTrue();
            genreResult.Success.Should().NotBeNull();
            genreResult.Success.Name.Should().Be(validName);
            genreResult.Success.Description.Should().Be(validDescription);
        }

        [Theory]
        [InlineData(null, "cannot be null or empty")]
        [InlineData("", "cannot be null or empty")]
        [InlineData(" ", "cannot be null or empty")]
        [InlineData("123456789", "must be at least 10 characters long")] 
        public void Create_WithInvalidDescription_ShouldReturnFailure(string invalidDescription, string expectedMessage)
        {
            // Arrange
            var validName = _faker.Lorem.Sentence(3);

            // Act
            var genreResult = Genre.Create(validName, invalidDescription);

            // Assert
            genreResult.IsFailure.Should().BeTrue();
            genreResult.Failure.Message.Should().Contain(expectedMessage);
        }

        [Fact]
        public void Create_WithDescriptionTooLong_ShouldReturnFailure()
        {
            // Arrange
            var longDescription = _faker.Random.String2(501);
            var validName = _faker.Lorem.Sentence(3);

            // Act
            var genreResult = Genre.Create(validName, longDescription);

            // Assert
            genreResult.IsFailure.Should().BeTrue();
            genreResult.Failure.Message.Should().Contain("description must not exceed 500 characters. Current length: 501");
        }

        [Theory]
        [InlineData(null, "cannot be null or empty")]
        [InlineData("", "cannot be null or empty")]
        [InlineData(" ", "cannot be null or empty")]
        [InlineData("Ab", "must be at least 3 characters long")] 
        public void Create_WithInvalidName_ShouldReturnFailure(string invalidName, string expectedMessage)
        {
            // Arrange
            var validDescription = _faker.Lorem.Sentence(10);

            // Act
            var genreResult = Genre.Create(invalidName, validDescription);

            // Assert
            genreResult.IsFailure.Should().BeTrue();
            genreResult.Failure.Message.Should().Contain(expectedMessage);
        }

        [Fact]
        public void Create_WithNameTooLong_ShouldReturnFailure()
        {
            // Arrange
            var longName = _faker.Random.String2(51);
            var validDescription = _faker.Lorem.Sentence(10);

            // Act
            var genreResult = Genre.Create(longName, validDescription);

            // Assert
            genreResult.IsFailure.Should().BeTrue();
            genreResult.Failure.Message.Should().Contain("name must not exceed 50 characters. Current length: 51");
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
