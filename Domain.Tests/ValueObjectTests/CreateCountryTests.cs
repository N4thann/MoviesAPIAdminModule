using Bogus;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjectTests
{
    public class CreateCountryTests
    {
        private readonly Faker _faker;
        private readonly string _validName;
        private readonly string _validCode;

        public CreateCountryTests()
        {
            _faker = new Faker("pt_BR");

            _validName = _faker.Address.Country();
            _validCode = _faker.Address.CountryCode();
        }


        [Fact]
        public void Create_WithValidData_ShouldReturnSuccess()
        {
            // Arrange & Act
            var countryResult = Country.Create(_validName, _validCode);

            // Assert
            countryResult.IsSuccess.Should().BeTrue();
            countryResult.Success.Should().NotBeNull();
            countryResult.Success.Name.Should().NotBeNullOrWhiteSpace();
            countryResult.Success.Code.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void Create_WithExtraSpacesAndLowercaseCode_ShouldTrimAndUppercaseCorrectly()
        {
            // Arrange & Act
            var countryResult = Country.Create("  United States  ", "  usa  ");

            // Assert
            countryResult.IsSuccess.Should().BeTrue();
            var country = countryResult.Success;
            country.Name.Should().Be("United States");
            country.Code.Should().Be("USA");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_WithNullOrEmptyName_ShouldReturnFailure(string invalidName)
        {
            // Arrange & Act
            var countryResult = Country.Create(invalidName, _validCode);

            // Assert
            countryResult.IsFailure.Should().BeTrue();
            countryResult.Failure.Message.Should().Contain("name");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_WithNullOrEmptyCode_ShouldReturnFailure(string invalidCode)
        {
            // Arrange & Act
            var countryResult = Country.Create(_validName, invalidCode);

            // Assert
            countryResult.IsFailure.Should().BeTrue();
            countryResult.Failure.Message.Should().Contain("code");
        }

        [Fact]
        public void Create_WithNameTooLong_ShouldReturnFailure()
        {
            // Arrange
            var longName = new string('A', 101);

            // Act
            var countryResult = Country.Create(longName, _validCode);

            // Assert
            countryResult.IsFailure.Should().BeTrue();
            countryResult.Failure.Message.Should().Contain("name");
        }

        [Fact]
        public void Create_WithCodeTooLong_ShouldReturnFailure()
        {
            // Arrange
            var longCode = "ABCD";

            // Act
            var countryResult = Country.Create(_validName, longCode);

            // Assert
            countryResult.IsFailure.Should().BeTrue();
            countryResult.Failure.Message.Should().Contain("code");
        }

        [Fact]
        public void TwoCountriesWithSameCode_ShouldBeEqual()
        {
            // Arrange
            var country1 = Country.Create("Estados Unidos", "USA").Success!;
            var country2 = Country.Create("United States of America", "USA").Success!;

            // Act & Assert
            country1.Should().Be(country2);
            (country1 == country2).Should().BeTrue();
        }

        [Fact]
        public void TwoCountriesWithDifferentCodes_ShouldNotBeEqual()
        {
            // Arrange
            var country1 = Country.Create("Brazil", "BR").Success!;
            var country2 = Country.Create("Germany", "DE").Success!;

            // Act & Assert
            country1.Should().NotBe(country2);
            (country1 == country2).Should().BeFalse();
        }

        [Fact]
        public void ToString_ShouldReturnCorrectlyFormattedString()
        {
            // Arrange
            var country = Country.Create("United States", "USA").Success!;
            var expectedString = "Name: United States / Code: USA";

            // Act
            var resultString = country.ToString();

            // Assert
            resultString.Should().Be(expectedString);
        }
    }
}
