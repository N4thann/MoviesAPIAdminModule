using Microsoft.AspNetCore.Http;
using static Domain.ValueObjects.MovieImage;

namespace Application.DTOs.Request.Movie
{
    public record class UploadImageRequest(
        IFormFile ImageFile,
        ImageType ImageType,
        string? AltText
        );
}
