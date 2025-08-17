using Amazon.S3;
using Amazon.S3.Model;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using static Domain.ValueObjects.MovieImage;

namespace Infraestructure.Service
{
    public class S3StorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _region;

        public S3StorageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration.GetValue<string>("AWS:BucketName")
                ?? throw new InvalidOperationException("AWS:BucketName is not configured.");
            _region = configuration.GetValue<string>("AWS:Region")
                ?? throw new InvalidOperationException("AWS:Region is not configured.");
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string originalFileName, string contentType, Movie movie, ImageType imageType)
        {
            var fileKey = await GenerateFileKeyAsync(originalFileName, movie, imageType);

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead 
            };

            await _s3Client.PutObjectAsync(putRequest);

            return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{fileKey}";
        }

        private async Task<string> GenerateFileKeyAsync(string originalFileName, Movie movie, ImageType imageType)
        {
            var fileExtension = Path.GetExtension(originalFileName);

            var movieFolder = $"{Slugify(movie.OriginalTitle)}-{movie.Id}";

            if (imageType == ImageType.Poster || imageType == ImageType.Thumbnail)
            {
                return $"{movieFolder}/{imageType.ToString().ToLower()}{fileExtension}";
            }
            else
            {
                var listRequest = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = $"{movieFolder}/{ImageType.Gallery.ToString().ToLower()}"
                };

                var response = await _s3Client.ListObjectsV2Async(listRequest);
                var newIndex = response.S3Objects.Count + 1;

                return $"{movieFolder}/{imageType.ToString().ToLower()}{newIndex:D2}{fileExtension}";
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
