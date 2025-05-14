using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Award : ValueObject
    {
        // Private constructor for EF Core
        private Award() { }

        public Award(string name, string institution, int year)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.NotNullOrEmpty(institution, nameof(institution));
            Validate.GreaterThan(year, 1880, nameof(year));
            Validate.LessThanOrEqualTo(year, DateTime.UtcNow.Year + 1, nameof(year)); // Allow next year

            Name = name.Trim();
            Institution = institution.Trim();
            Year = year;
        }

        public string Name { get; private set; }
        public string Institution { get; private set; }
        public int Year { get; private set; }

        /// <summary>
        /// Define the components used for equality comparison
        /// Two awards are equal if they have the same name, institution, and year
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name.ToLowerInvariant();
            yield return Institution.ToLowerInvariant();
            yield return Year;
        }

        /// <summary>
        /// Returns a formatted string representation of the award
        /// </summary>
        public override string ToString()
        {
            return $"{Name} ({Institution}, {Year})";
        }

        /// <summary>
        /// Creates an Award for Oscar
        /// </summary>
        public static Award CreateOscar(string category, int year)
        {
            return new Award($"Oscar - {category}", "Academy of Motion Picture Arts and Sciences", year);
        }

        /// <summary>
        /// Creates an Award for Golden Globe
        /// </summary>
        public static Award CreateGoldenGlobe(string category, int year)
        {
            return new Award($"Golden Globe - {category}", "Hollywood Foreign Press Association", year);
        }

        /// <summary>
        /// Creates an Award for Emmy
        /// </summary>
        public static Award CreateEmmy(string category, int year)
        {
            return new Award($"Emmy - {category}", "Academy of Television Arts & Sciences", year);
        }

        /// <summary>
        /// Validates if the award name matches known award patterns
        /// </summary>
        public bool IsPrestigiousAward()
        {
            var prestigiousInstitutions = new[]
            {
            "Academy of Motion Picture Arts and Sciences",
            "Hollywood Foreign Press Association",
            "Cannes Film Festival",
            "Venice International Film Festival",
            "Berlin International Film Festival",
            "British Academy of Film and Television Arts"
        };

            return prestigiousInstitutions.Any(institution =>
                Institution.Equals(institution, StringComparison.OrdinalIgnoreCase));
        }
    }
}
