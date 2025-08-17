using Domain.Entities;
using Domain.SeedWork;
using Domain.SeedWork.Validation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            Url = url;
            AltText = altText;
            Type = type;
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

        public enum ImageType
        {
            Poster = 1,
            Gallery = 2,
            Thumbnail = 3
        }
    }
}
