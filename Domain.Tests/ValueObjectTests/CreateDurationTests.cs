
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjectTests
{
    public class CreateDurationTests
    {
        [Fact]
        public void Create_WithValidDuration_ShouldReturnSuccess()
        {
            // Arrange & Act
            var durationResult = Duration.Create(120);

            // Assert
            durationResult.IsSuccess.Should().BeTrue();
            durationResult.Success.Should().NotBeNull();
            durationResult.Success.Minutes.Should().Be(120);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)] 
        [InlineData(601)] 
        public void Create_WithDurationOutOfRange_ShouldReturnFailure(int invalidMinutes)
        {
            // Arrange & Act
            var durationResult = Duration.Create(invalidMinutes);

            // Assert
            durationResult.IsFailure.Should().BeTrue();
            durationResult.Failure.Code.Should().Be(400);
            durationResult.Failure.Message.Should().Contain("minutes");
        }

        [Fact]
        public void CalculatedProperties_ForExactHoursDuration_ShouldBeCorrect()
        {
            // Arrange
            var duration = Duration.Create(180).Success!;

            // Act & Assert
            duration.Hours.Should().Be(3);
            duration.RemainingMinutes.Should().Be(0);
        }

        [Fact]
        public void CalculatedProperties_ForDurationWithRemainingMinutes_ShouldBeCorrect()
        {
            // Arrange
            var duration = Duration.Create(95).Success!;

            // Act & Assert
            duration.Hours.Should().Be(1);
            duration.RemainingMinutes.Should().Be(35);
        }

        [Fact]
        public void CalculatedProperties_ForDurationLessThanOneHour_ShouldBeCorrect()
        {
            // Arrange
            var duration = Duration.Create(45).Success!;

            // Act & Assert
            duration.Hours.Should().Be(0);
            duration.RemainingMinutes.Should().Be(45);
        }

        [Fact]
        public void TwoDurationsWithSameMinutes_ShouldBeEqual()
        {
            // Arrange
            var duration1 = Duration.Create(150).Success!;
            var duration2 = Duration.Create(150).Success!;

            // Act & Assert
            duration1.Should().Be(duration2);
            (duration1 == duration2).Should().BeTrue();
            (duration1 != duration2).Should().BeFalse();
        }

        [Fact]
        public void TwoDurationsWithDifferentMinutes_ShouldNotBeEqual()
        {
            // Arrange
            var duration1 = Duration.Create(150).Success!;
            var duration2 = Duration.Create(151).Success!;

            // Act & Assert
            duration1.Should().NotBe(duration2);
            (duration1 == duration2).Should().BeFalse();
            (duration1 != duration2).Should().BeTrue();
        }

        [Theory]
        [InlineData(59, "59min")]                           // Caso 1: Apenas minutos
        [InlineData(120, "2h")]                             // Caso 2: Apenas horas (exatas)
        [InlineData(125, "2h 5min")]                        // Caso 3: Horas e minutos
        [InlineData(60, "1h")]                              // Caso de borda: 60 minutos exatos
                                                            // ToString_ParaDiferentesDuracoes_DeveRetornarStringFormatadaCorretamente
        public void ToString_ForVariousDurations_ShouldReturnCorrectlyFormattedString(int minutes, string expectedFormat)
        {
            // Arrange
            var duration = Duration.Create(minutes).Success!;

            // Act
            var formattedString = duration.ToString();

            // Assert
            formattedString.Should().Be(expectedFormat);
        }
    }
}
