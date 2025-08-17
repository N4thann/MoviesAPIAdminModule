using Application.Interfaces;
using static Domain.ValueObjects.MovieImage;

namespace Application.Commands.Movie
{
    /// <summary>
    /// Comando para adicionar uma imagem a um filme existente.
    /// </summary>
    /// <param name="MovieId">O ID do filme que receberá a imagem.</param>
    /// <param name="FileStream">O fluxo de bytes do arquivo de imagem.</param>
    /// <param name="OriginalFileName">O nome original do arquivo para extrair a extensão.</param>
    /// <param name="ContentType">O tipo de conteúdo do arquivo (ex: "image/jpeg").</param>
    /// <param name="ImageType">O tipo da imagem (Poster, Thumbnail, ou Gallery).</param>
    /// <param name="AltText">O texto alternativo para a imagem.</param>
    /// <returns>A URL pública da imagem salva.</returns>
    public record AddMovieImageCommand(
        Guid MovieId,
        Stream FileStream,
        string OriginalFileName,
        string ContentType,
        ImageType ImageType,
        string? AltText
    ) : ICommand<string>;
}
