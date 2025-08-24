using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.ValueObjects;

namespace Domain.SeedWork.Interfaces
{
    public interface IFileStorageService
    {
        Task<Result<string>> SaveFileAsync(Stream fileStream, string originalFileName, string contentType, Movie movie, MovieImage.ImageType imageType);
    }
}
