using Domain.SeedWork;
using Domain.SeedWork.Validation;

namespace Domain.ValueObjects
{
    public class Award : ValueObject
    {
        private const int MIN_YEAR = 1880;
        private static readonly int MAX_YEAR = DateTime.UtcNow.Year + 1;

        private Award() { }

        public Award(AwardCategory category, Institution institution, int year)
        {
            ValidateInputs(category, institution, year);

            Category = category;
            Institution = institution;
            Year = year;
        }

        public AwardCategory Category { get; private set; }
        public Institution Institution { get; private set; }
        public int Year { get; private set; }

        private static void ValidateInputs(AwardCategory category, Institution institution, int year)
        {
            Validate.NotNull(category, nameof(category));
            Validate.NotNull(institution, nameof(institution));
            Validate.Range(year, MIN_YEAR, MAX_YEAR, nameof(year));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Category;
            yield return Institution;
            yield return Year;
        }

        public override string ToString()
        {
            return $"{Category.Name} ({Institution.Name}, {Year})";
        }
    }
}