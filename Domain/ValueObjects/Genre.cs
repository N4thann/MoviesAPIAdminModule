using Domain.SeedWork;
using Domain.SeedWork.Validation;

namespace Domain.ValueObjects
{
    public class Genre : ValueObject
    {
        public Genre() { }

        public Genre(string name, string description = null)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.MaxLength(name, 50, nameof(name));

            if (!string.IsNullOrWhiteSpace(description))
            {
                Validate.MaxLength(description, 500, nameof(description));
            }

            Name = name.Trim();

        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name.ToLowerInvariant();
        }

        public override string ToString() => Name;

        // Factory methods
        public static Genre Action => new("Action", "Fast-paced films with physical stunts and challenges");
        public static Genre Drama => new("Drama", "Character-driven stories with emotional themes");
        public static Genre Comedy => new("Comedy", "Humorous films intended to entertain and amuse");
        public static Genre Horror => new("Horror", "Films intended to frighten and create suspense");
        public static Genre Romance => new("Romance", "Films focused on love stories and relationships");
        public static Genre SciFi => new("Science Fiction", "Films featuring futuristic or technological themes");
        public static Genre Fantasy => new("Fantasy", "Films with magical or supernatural elements");
        public static Genre Thriller => new("Thriller", "Suspenseful films with tension and excitement");
        public static Genre Animation => new("Animation", "Films created using animated techniques");
        public static Genre Documentary => new("Documentary", "Non-fiction films presenting factual content");
    }
}
