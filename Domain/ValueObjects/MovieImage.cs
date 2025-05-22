using Domain.SeedWork;
using Domain.SeedWork.Validation;

namespace Domain.ValueObjects
{
    public class MovieImage : ValueObject
    {
        public MovieImage() { }

        public MovieImage(string url, string altText = null, ImageType type = ImageType.Gallery)
        {
            Validate.NotNullOrEmpty(url, nameof(url));
            Validate.IsValidUrl(url, nameof(url));

            if (!string.IsNullOrEmpty(altText))
            {
                Validate.MaxLength(altText, 200, nameof(altText));
            }
        }
        public string Url { get; private set; }
        public string AltText { get; private set; }
        public ImageType Type { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Url;
            yield return Type;
        }

        public override string ToString() => $"{Type}: {Url}";

        // Factory methods para diferentes tipos de imagem
        public static MovieImage CreatePoster(string url, string altText = null) 
            => new MovieImage(url, altText, ImageType.Poster);

        public static MovieImage CreateGallery(string url, string altText = null)
            => new(url, altText, ImageType.Gallery);

        public static MovieImage CreateThumbnail(string url, string altText = null)
            => new(url, altText, ImageType.Thumbnail);

        public enum ImageType
        {
            Poster = 1,
            Gallery = 2,
            Thumbnail = 3
        }
    }
}
