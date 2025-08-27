using Application.Commands.Movie;
using Application.Interfaces;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;
using static Domain.ValueObjects.MovieImage;

public class AddMovieImageUseCase : ICommandHandler<AddMovieImageCommand, Result<string>>
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

    public async Task<Result<string>> Handle(AddMovieImageCommand command, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdWithImagesAsync(command.Id);

        if (movie is null)
            return Result<string>.AsFailure(Failure.NotFound("Movie", command.Id));

        var imageResult = await _fileStorageService.SaveFileAsync(
            command.FileStream,
            command.OriginalFileName,
            command.ContentType,
            movie,
            command.ImageType
        );

        if (imageResult.IsFailure)
            return Result<string>.AsFailure(imageResult.Failure!);

        var image = imageResult.Success!;

        var movieImageResult = MovieImage.Create(image, command.AltText, command.ImageType);

        if (movieImageResult.IsFailure)
            return Result<string>.AsFailure(movieImageResult.Failure!);

        var newMovieImage = movieImageResult.Success!;
        Result<bool> domainResult;

        switch (command.ImageType)
        {
            case ImageType.Poster:
                domainResult = movie.SetPoster(newMovieImage);
                break;
            case ImageType.Thumbnail:
                domainResult = movie.SetThumbnail(newMovieImage);
                break;
            case ImageType.Gallery:
                domainResult = movie.AddGalleryImage(newMovieImage);
                break;
            default:
                return Result<string>.AsFailure(Failure.Validation("Unknown image type specified."));
        }

        if (domainResult.IsFailure)
            return Result<string>.AsFailure(domainResult.Failure!);

        await _unitOfWork.Commit(cancellationToken);

        return Result<string>.AsSuccess(image);
    }
}
