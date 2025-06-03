using Domain.SeedWork.Validation;
using Domain.ValueObjects;
using MoviesAPIAdminModule.Domain.SeedWork;

namespace Domain.Entities
{

    public class Director : BaseEntity
    {
        private const int MAX_BIO_LENGTH = 1000;

        protected Director() { }

        public Director(
            string name,
            DateTime birthDate,
            Country country,
            string? biography = null,
            Gender gender = Gender.NotSpecified) : this() 
        {
            ValidateConstructorInputs(name, birthDate, country, biography);

            Name = name.Trim();
            BirthDate = birthDate.Date;
            Country = country;
            Biography = biography?.Trim();
            Gender = gender;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Propriedades específicas do Diretor
        public DateTime BirthDate { get; private set; }
        public Country Country { get; private set; } 
        public string? Biography { get; private set; }
        public Gender Gender { get; private set; } 


        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Propriedades calculadas
        public int Age => CalculateAge(BirthDate);


        public void UpdateBasicInfo(string name, string? biography = null, Gender gender = Gender.NotSpecified)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.MaxLength(name, 50, nameof(name));
            if (!string.IsNullOrWhiteSpace(biography))
            {
                Validate.MaxLength(biography, MAX_BIO_LENGTH, nameof(biography));
            }

            Name = name.Trim();
            Biography = biography?.Trim();
            Gender = gender;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateBirthDate(DateTime newBirthDate)
        {
            Validate.IsPastDate(newBirthDate, nameof(newBirthDate), allowToday: false); 
            BirthDate = newBirthDate.Date;
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

        #region Métodos de Validação Interna

        private static void ValidateConstructorInputs(
            string name,
            DateTime birthDate,
            Country country,
            string? biography)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.MaxLength(name, 50, nameof(name));

            Validate.IsPastDate(birthDate, nameof(birthDate), allowToday: false); 

            Validate.NotNull(country, nameof(country));

            if (!string.IsNullOrWhiteSpace(biography))
            {
                Validate.MaxLength(biography, MAX_BIO_LENGTH, nameof(biography));
            }
        }

        #endregion

        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public override string ToString()
        {
            return $"{Name} ({Age} anos) - {Country.Name}";
        }
    }

    public enum Gender
    {
        NotSpecified = 0,
        Male = 1,
        Female = 2,
        Other = 3
    }
}