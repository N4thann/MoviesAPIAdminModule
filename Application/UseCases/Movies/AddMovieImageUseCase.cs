using Application.Commands.Movie;
using Application.Interfaces;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;
using static Domain.ValueObjects.MovieImage;

public class AddMovieImageUseCase : ICommandHandler<AddMovieImageCommand, string>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public AddMovieImageUseCase(
        IMovieRepository movieRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork)
    {
        _movieRepository = movieRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(AddMovieImageCommand command, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdWithImagesAsync(command.MovieId);

        if (movie is null)
        {
            throw new KeyNotFoundException($"Movie with ID {command.MovieId} not found.");
        }

        var imageUrl = await _fileStorageService.SaveFileAsync(
            command.FileStream,
            command.OriginalFileName,
            command.ContentType,
            movie,
            command.ImageType
        );

        var newMovieImage = new MovieImage(imageUrl, command.AltText, command.ImageType);

        switch (command.ImageType)
        {
            case ImageType.Poster:
                movie.SetPoster(newMovieImage);
                break;
            case ImageType.Thumbnail:
                movie.SetThumbnail(newMovieImage);
                break;
            case ImageType.Gallery:
                movie.AddGalleryImage(newMovieImage);
                break;
            default:
                throw new InvalidOperationException("Unknown image type specified.");
        }

        await _unitOfWork.Commit(cancellationToken);

        return imageUrl;
    }
}
