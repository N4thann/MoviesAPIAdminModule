using Domain.SeedWork.Core;
using Domain.ValueObjects;
using FluentAssertions;
using Infraestructure.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text;
using TestsCommon;

namespace Infraestructure.Tests
{
    public class LocalStorageServiceTests : IDisposable
    {
        private readonly LocalStorageService _service;
        private readonly string _testRootPath; // O diretório raiz temporário para nossos testes
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        public LocalStorageServiceTests()
        {
            // 1. --- SETUP DO AMBIENTE DE TESTE ---

            // Cria um diretório temporário único para esta execução de teste.
            _testRootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testRootPath);

            // 2. --- MOCK DAS DEPENDÊNCIAS ---

            // Mock IWebHostEnvironment para apontar para nossa pasta de teste.
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.ContentRootPath).Returns(_testRootPath);

            // Mock IConfiguration para fornecer o caminho de upload.
            var inMemorySettings = new Dictionary<string, string> {
            {"FileStorageSettings:LocalUploadPath", "StaticFiles/Images"}
        };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Mock IHttpContextAccessor para simular uma requisição web.
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7211");
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            _service = new LocalStorageService(configuration, _mockHttpContextAccessor.Object, mockEnv.Object);
        }

        [Fact]
        public async Task SaveFileAsync_ForPoster_ShouldCreateDirectoryAndSaveFileWithCorrectName()
        {
            // Arrange
            var movie = TestDataFactory.CreateInceptionMovie().Success!;
            var fileContent = "Este é o conteúdo de um poster falso.";
            var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            var originalFileName = "poster_inception.jpg";

            var expectedSlug = $"inception-{movie.Id}";
            var expectedDirectoryPath = Path.Combine(_testRootPath, "StaticFiles", "Images", expectedSlug);
            var expectedFileName = $"inception_{movie.ReleaseYear}_Poster.jpg";
            var expectedFilePath = Path.Combine(expectedDirectoryPath, expectedFileName);
            var expectedUrl = $"https://localhost:7211/StaticFiles/Images/{expectedSlug}/{expectedFileName}";

            // Act
            var result = await _service.SaveFileAsync(fileStream, originalFileName, "image/jpeg", movie, MovieImage.ImageType.Poster);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Success.Should().Be(expectedUrl);

            Directory.Exists(expectedDirectoryPath).Should().BeTrue();
            File.Exists(expectedFilePath).Should().BeTrue();
            (await File.ReadAllTextAsync(expectedFilePath)).Should().Be(fileContent);
        }

        [Fact]
        public async Task SaveFileAsync_ForGalleryImages_ShouldIncrementIndexInFileName()
        {
            // Arrange
            var movie = TestDataFactory.CreateInceptionMovie().Success!;
            var fileStream1 = new MemoryStream(Encoding.UTF8.GetBytes("img1"));
            var fileStream2 = new MemoryStream(Encoding.UTF8.GetBytes("img2"));

            var expectedSlug = $"inception-{movie.Id}";
            var expectedDirectoryPath = Path.Combine(_testRootPath, "StaticFiles", "Images", expectedSlug);

            // Nomes de arquivo esperados com índice
            var expectedFileName1 = $"inception_{movie.ReleaseYear}_Gallery01.png";
            var expectedFileName2 = $"inception_{movie.ReleaseYear}_Gallery02.png";

            var expectedFilePath1 = Path.Combine(expectedDirectoryPath, expectedFileName1);
            var expectedFilePath2 = Path.Combine(expectedDirectoryPath, expectedFileName2);

            // Act
            await _service.SaveFileAsync(fileStream1, "gallery1.png", "image/png", movie, MovieImage.ImageType.Gallery);
            await _service.SaveFileAsync(fileStream2, "gallery2.png", "image/png", movie, MovieImage.ImageType.Gallery);

            // Assert
            File.Exists(expectedFilePath1).Should().BeTrue();
            File.Exists(expectedFilePath2).Should().BeTrue();
        }

        [Fact]
        // SalvarArquivo_QuandoOcorreExcecao_DeveRetornarFalhaDeInfraestrutura
        public async Task SaveFileAsync_WhenExceptionOccurs_ShouldReturnInfrastructureFailure()
        {
            // Arrange
            var movie = TestDataFactory.CreateInceptionMovie().Success!;
            var fileStream = new MemoryStream();

            // Simula um erro grave (neste caso, o HttpContext é nulo, o que causará uma NullReferenceException)
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext)null);

            // Act
            var result = await _service.SaveFileAsync(fileStream, "file.jpg", "image/jpeg", movie, MovieImage.ImageType.Poster);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().Be(Failure.InfrastructureError("Failed to save file to local storage."));
        }

        public void Dispose()
        {
            if (Directory.Exists(_testRootPath))
            {
                Directory.Delete(_testRootPath, recursive: true);
            }
        }
    }
}
