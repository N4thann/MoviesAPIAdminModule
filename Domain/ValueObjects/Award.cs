using Domain.SeedWork;
using Domain.SeedWork.Validation;
using System.ComponentModel.DataAnnotations;

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

        public static Award CreateOscar(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Oscar - {category}", "Academy of Motion Picture Arts and Sciences", year);
        }

        public static Award CreateGoldenGlobe(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Golden Globe - {category}", "Hollywood Foreign Press Association", year);
        }

        public static Award CreateBAFTA(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"BAFTA - {category}", "British Academy of Film and Television Arts", year);
        }

        public static Award CreateSAGAAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"SAG Award - {category}", "Screen Actors Guild", year);
        }

        public static Award CreateEmmyAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Emmy - {category}", "Academy of Television Arts & Sciences", year);
        }

        public static Award CreateCannesAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Cannes - {category}", "Cannes Film Festival", year);
        }

        public static Award CreateVeniceAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Venice Award - {category}", "Venice International Film Festival", year);
        }

        public static Award CreateBerlinAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Berlin Award - {category}", "Berlin International Film Festival", year);
        }

        public static Award CreateSundanceAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Sundance Award - {category}", "Sundance Film Festival", year);
        }

        public static Award CreateCriticsChoiceAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Critics Choice Award - {category}", "Critics Choice Association", year);
        }

        public static Award CreateAnnieAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Annie Award - {category}", "International Animated Film Association", year);
        }

        public static Award CreateSaturnAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Saturn Award - {category}", "Academy of Science Fiction, Fantasy and Horror Films", year);
        }

        public static Award CreatePeoplesChoiceAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"People's Choice Award - {category}", "People's Choice Awards", year);
        }

        public static Award CreateIndependentSpiritAward(string category, int year)
        {
            ValidateCategoryAndYear(category, year);
            return new Award($"Independent Spirit Award - {category}", "Film Independent", year);
        }

        private static void ValidateCategoryAndYear(string category, int year)
        {
            Validate.NotNullOrEmpty(category, nameof(category));
            Validate.MaxLength(category, 100, nameof(category));
            Validate.Range(year, MIN_YEAR, MAX_YEAR, nameof(year));
        }

        public string GetAwardType()
        {
            return Institution switch
            {
                "Academy of Motion Picture Arts and Sciences" => "Academy Awards",
                "Hollywood Foreign Press Association" => "Golden Globes",
                "British Academy of Film and Television Arts" => "BAFTA",
                "Screen Actors Guild" => "SAG Awards",
                "Academy of Television Arts & Sciences" => "Emmy Awards",
                "Cannes Film Festival" => "Cannes Festival",
                "Venice International Film Festival" => "Venice Festival",
                "Berlin International Film Festival" => "Berlin Festival",
                "Sundance Film Festival" => "Sundance Festival",
                "Critics Choice Association" => "Critics Choice Awards",
                "International Animated Film Association" => "Annie Awards",
                "Academy of Science Fiction, Fantasy and Horror Films" => "Saturn Awards",
                "People's Choice Awards" => "People's Choice",
                "Film Independent" => "Independent Spirit Awards",
                _ => "Other Film Award"
            };
        }

        // Método auxiliar para obter todas as instituições disponíveis
        public static IReadOnlyList<string> GetAvailableInstitutions()
        {
            return new List<string>
            {
                "Academy of Motion Picture Arts and Sciences",
                "Hollywood Foreign Press Association",
                "British Academy of Film and Television Arts",
                "Screen Actors Guild",
                "Academy of Television Arts & Sciences",
                "Cannes Film Festival",
                "Venice International Film Festival",
                "Berlin International Film Festival",
                "Sundance Film Festival",
                "Critics Choice Association",
                "International Animated Film Association",
                "Academy of Science Fiction, Fantasy and Horror Films",
                "People's Choice Awards",
                "Film Independent"
            };
        }
    }
}