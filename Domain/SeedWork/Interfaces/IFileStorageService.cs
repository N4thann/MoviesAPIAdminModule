using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.SeedWork.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Saves a file to the configured storage and returns its public URL.
        /// </summary>
        /// <returns>The public URL of the saved file.</returns>
        Task<string> SaveFileAsync(Stream fileStream, string originalFileName, string contentType, Movie movie, MovieImage.ImageType imageType);
    }
}
