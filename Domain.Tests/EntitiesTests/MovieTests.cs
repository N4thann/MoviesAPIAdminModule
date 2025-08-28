using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.EntitiesTests
{
    public class MovieTests
    {
        [Fact]
        public void Create_WithValidData_ReturnsSuccessResult() //MethodName_Scenario_ExpectedBehavior
        {
            // Arrange & Act
            var inceptionResult = TestDataFactory.CreateInceptionMovie();

            // Assert
            inceptionResult.IsSuccess.Should().BeTrue();

            var movie = inceptionResult.Success!;
            movie.Name.Should().Be("A Origem");
            movie.Awards.Should().HaveCount(1);
            movie.Images.Should().HaveCount(1);
            movie.HasPoster.Should().BeTrue();
        }

        [Fact]
        public void Create_WithEmptyTitle_ReturnsValidationFailure()
        {
            // Arrange
            var director = TestDataFactory.ChristopherNolan();
            var studio = TestDataFactory.WarnerBros();

            var durationResult = Duration.Create(148);
            var countryResult = Country.Create("United States", "USA");

            // Act
            var result = Movie.Create(
                title: "",
                originalTitle: "Inception", synopsis: "Synopsis", releaseYear: 2010,
                duration: durationResult.Success!, country: countryResult.Success!,
                studio: studio.Success!, director: director.Success!, genre: new Genre("Sci-Fi", "...")
            );

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Message.Should().Contain("title cannot be null or empty.");
        }

        [Fact]
        public void UpdateBasicInfo_WithValidData_ReturnsSuccessAndUpdateProperties()
        {
            var movie = TestDataFactory.CreateInceptionMovie().Success!;
            var originalUpdatedAt = movie.UpdatedAt;

            var durationResult = Duration.Create(155);

            // 2. Definimos os novos dados para a atualização
            var newTitle = "Inception Remastered";
            var newOriginalTitle = "Inception: The Director's Cut";
            var newSynopsis = "An updated synopsis.";
            var newDuration = durationResult.Success;
            var newReleaseYear = 2025;

            // Act
            var updateResult = movie.UpdateBasicInfo(newTitle, newOriginalTitle, newSynopsis, newDuration, newReleaseYear);

            // Assert
            updateResult.IsSuccess.Should().BeTrue();
            movie.Name.Should().Be(newTitle);
            movie.Synopsis.Should().Be(newSynopsis);
            movie.Duration.Should().Be(newDuration);
            movie.ReleaseYear.Should().Be(newReleaseYear);
            movie.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public void UpdateBasicInfo_WithEmptyTitle_ReturnsValidationFailureAndDoesNotChangeState()
        {
            // Arrange
            // 1. Pegamos um filme válido da nossa fábrica
            var movie = TestDataFactory.CreateInceptionMovie().Success!;
            var originalTitle = movie.Name; // Guardamos os valores originais
            var originalSynopsis = movie.Synopsis;
            var durationResult = Duration.Create(155);;


            // 2. Definimos um título inválido para o teste
            var invalidTitle = "";

            // Act
            // 3. Tentamos atualizar com o dado inválido
            var updateResult = movie.UpdateBasicInfo(invalidTitle, "New OT", "New Synopsis", durationResult.Success, 2025);

            // Assert
            // 4. Verificamos se a operação falhou
            updateResult.IsFailure.Should().BeTrue();
            updateResult.Failure.Message.Should().Contain("title cannot be null or empty.");

            // 5. IMPORTANTE: Verificamos se o estado da entidade NÃO foi alterado
            movie.Name.Should().Be(originalTitle);
            movie.Synopsis.Should().Be(originalSynopsis);
        }
    }
}
