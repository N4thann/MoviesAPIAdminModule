using Domain.Entities;

namespace Domain.SeedWork.Interfaces
{
    public interface IMovieRepository
    {
        /// <summary>
        /// Gets a movie by its ID, including its related images collection.
        /// </summary>
        /// <param name="movieId">The movie's unique identifier.</param>
        /// <returns>The Movie aggregate root with its images, or null if not found.</returns>
        Task<Movie?> GetByIdWithImagesAsync(Guid movieId);

        /// <summary>
        /// Gets a movie by its ID, including its related awards collection.
        /// </summary>
        /// <param name="movieId">The movie's unique identifier.</param>
        /// <returns>The Movie aggregate root with its awards, or null if not found.</returns>
        Task<Movie?> GetByIdWithAwardAsync(Guid movieId);
    }
}
