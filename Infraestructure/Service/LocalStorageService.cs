using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using static Domain.ValueObjects.MovieImage;

namespace Infraestructure.Service
{
    public class LocalStorageService : IFileStorageService
    {
        private readonly string _storagePath;
        private readonly string _requestPath;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalStorageService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _httpContextAccessor = httpContextAccessor;

            _requestPath = configuration.GetValue<string>("FileStorageSettings:LocalUploadPath")
                ?? throw new InvalidOperationException("FileStorageSettings:LocalUploadPath is not configured.");

            _storagePath = Path.Combine(env.ContentRootPath, _requestPath);
        }

        public async Task<Result<string>> SaveFileAsync(Stream fileStream, string originalFileName, string contentType, Movie movie, MovieImage.ImageType imageType)
        {
            try
            {
                var movieDirectorySlug = $"{Slugify(movie.OriginalTitle)}-{movie.Id}";
                var movieDirectoryPath = Path.Combine(_storagePath, movieDirectorySlug);

                if (!Directory.Exists(movieDirectoryPath))
                {
                    Directory.CreateDirectory(movieDirectoryPath);
                }

                var fileName = GenerateFileName(originalFileName, movie, imageType, movieDirectoryPath);
                var absolutePath = Path.Combine(movieDirectoryPath, fileName);

                await using var stream = new FileStream(absolutePath, FileMode.Create);
                await fileStream.CopyToAsync(stream);

                var request = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";

                var finalUrlPath = $"{_requestPath}/{movieDirectorySlug}/{fileName}".Replace("\\", "/");

                return Result<string>.AsSuccess($"{baseUrl}/{finalUrlPath}");
            }
            catch
            {
                return Result<string>.AsFailure(Failure.Infrastructure("Failed to save file to local storage."));
            }       
        }

        private string GenerateFileName(string originalFileName, Movie movie, MovieImage.ImageType imageType, string movieDirectoryPath)
        {
            var movieSlug = Slugify(movie.OriginalTitle);
            var fileExtension = Path.GetExtension(originalFileName);

            if (imageType == ImageType.Poster || imageType == ImageType.Thumbnail)
            {
                return $"{movieSlug}_{movie.ReleaseYear}_{imageType}{fileExtension}";
            }
            else
            {
                var existingGalleryImages = Directory.GetFiles(movieDirectoryPath, $"*{ImageType.Gallery.ToString()}*").Length;
                var newIndex = existingGalleryImages + 1;

                return $"{movieSlug}_{movie.ReleaseYear}_{imageType}{newIndex:D2}{fileExtension}";
            }
        }
      
        private string Slugify(string text)
        {
            text = text.ToLowerInvariant();
            var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            text = stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"[^a-z0-9\-]", "");
            text = Regex.Replace(text, @"-+", "-");
            text = text.Trim('-');
            return text;
        }
    }
}
