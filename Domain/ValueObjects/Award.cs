using Domain.SeedWork;
using Domain.SeedWork.Validation;

namespace Domain.ValueObjects
{
    public class Award : ValueObject
    {
        private const int MIN_YEAR = 1880;
        private static readonly int MAX_YEAR = DateTime.UtcNow.Year + 1;
        private const int MAX_NAME_LENGTH = 200;
        private const int MAX_INSTITUTION_LENGTH = 200;

        private Award() { }

        public Award(string name, string institution, int year)
        {
            ValidateInputs(name, institution, year);

            Name = name.Trim();
            Institution = institution.Trim();
            Year = year;
        }

        public string Name { get; private set; }
        public string Institution { get; private set; }
        public int Year { get; private set; }

        private static void ValidateInputs(string name, string institution, int year)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.MaxLength(name, MAX_NAME_LENGTH, nameof(name));

            Validate.NotNullOrEmpty(institution, nameof(institution));
            Validate.MaxLength(institution, MAX_INSTITUTION_LENGTH, nameof(institution));

            Validate.Range(year, MIN_YEAR, MAX_YEAR, nameof(year));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name.ToLowerInvariant();
            yield return Institution.ToLowerInvariant();
            yield return Year;
        }

        public override string ToString()
        {
            return $"{Name} ({Institution}, {Year})";
        }

        // Método auxiliar para obter todas as instituições disponíveis
        public static IReadOnlyList<string> GetAvailableInstitutions()
        {
            return new List<string>
            {
                "Academy of Motion Picture Arts and Sciences (Academy Awards)",
                "Hollywood Foreign Press Association (Golden Globes)",
                "British Academy of Film and Television Arts (BAFTA)",
                "Screen Actors Guild (SAG Awards)",
                "Academy of Television Arts & Sciences (Emmy Awards)",
                "Cannes Film Festival (Cannes Festival)",
                "Venice International Film Festival (Venice Festival)",
                "Berlin International Film Festival (Berlin Festival)",
                "Sundance Film Festival (Sundance Festival)",
                "Critics Choice Association (Critics Choice Awards)",
                "International Animated Film Association (Annie Awards)",
                "Academy of Science Fiction, Fantasy and Horror Films (Saturn Awards)",
                "People's Choice Awards (People's Choice)",
                "Film Independent (Independent Spirit Awards)"
            };
        }
    }
}