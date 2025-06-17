using Domain.SeedWork.Validation;
using Domain.ValueObjects;
using MoviesAPIAdminModule.Domain.SeedWork;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Studio : BaseEntity
    {
        private const int MAX_HISTORY_LENGTH = 2000;

        protected Studio() { }

        public Studio(string name, Country country, DateTime foundationDate, string? history = null) : this() 
        {
            ValidateConstructorInputs(name, country, foundationDate, history);

            Name = name.Trim();
            Country = country;
            FoundationDate = foundationDate.Date; 
            History = history?.Trim();
            IsActive = true; 
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Propriedades principais
        public Country Country { get; private set; }
        public DateTime FoundationDate { get; private set; }
        public string? History { get; private set; }

        // Propriedades de controle (padrão)
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Propriedades calculadas (exemplo)
        public int YearsInOperation => CalculateYearsInOperation(FoundationDate);

        #region Métodos de Validação

        private static void ValidateConstructorInputs(
            string name,           
            Country country,
            DateTime foundationDate,
            string? history
            )
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.MaxLength(name, 100, nameof(name));

            Validate.NotNull(country, nameof(country));

            Validate.IsPastDate(foundationDate, nameof(foundationDate), allowToday: true);

            if (!string.IsNullOrWhiteSpace(history))
            {
                Validate.MaxLength(history, MAX_HISTORY_LENGTH, nameof(history));
            }
        }

        private static void ValidateBasicInfoUpdate(string name, string? history)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.MaxLength(name, 50, nameof(name));

            if (!string.IsNullOrWhiteSpace(history))
            {
                Validate.MaxLength(history, MAX_HISTORY_LENGTH, nameof(history));
            }
        }
        #endregion

        #region Métodos de Negócio

        public void UpdateBasicInfo(string name, string? history = null)
        {
            ValidateBasicInfoUpdate(name, history);

            Name = name.Trim();
            History = history?.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateCountry(Country country)
        {
            Validate.NotNull(country, nameof(country));
            Country = country;
            UpdatedAt = DateTime.Now;
        }
        
        public void UpdateFoundationDate(DateTime foundationDate)
        {
            Validate.IsPastDate(foundationDate, nameof(foundationDate), allowToday: true);

            FoundationDate = foundationDate.Date;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
        #endregion

        private static int CalculateYearsInOperation(DateTime foundationDate)
        {
            var today = DateTime.Today;
            var years = today.Year - foundationDate.Year;
            if (foundationDate.Date > today.AddYears(-years)) years--;

            return years;
        }
        public override string ToString()
        {
            return $"{Name} (Fundado em {FoundationDate.Year}) - {Country.Name}";
        }
    }
}
