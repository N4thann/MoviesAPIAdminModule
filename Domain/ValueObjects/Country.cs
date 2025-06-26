using Domain.SeedWork;
using Domain.SeedWork.Validation;


namespace Domain.ValueObjects
{
    public class Country : ValueObject
    {
        public Country() { }

        public Country(string name, string code) 
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.NotNullOrEmpty(code, nameof(code));
            Validate.MaxLength(name, 100, nameof(name));
            Validate.MaxLength(code, 3, nameof(code));

            Name = name.Trim();
            Code = code.Trim().ToUpperInvariant();
        }

        public string Name { get; set; }
        public string Code { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        public override string ToString() => Name;

        //Factory Methods
        public static Country Brazil => new("Brazil", "BR");
        public static Country UnitedStates => new("United States", "USA");
        public static Country UnitedKingdom => new("United Kingdom", "GB");
        public static Country France => new("France", "FR");
        public static Country Germany => new("Germany", "DE");
        public static Country Japan => new("Japan", "JP");
        public static Country China => new("China", "CN");
        public static Country India => new("India", "IN");
        public static Country Canada => new("Canada", "CA");
        public static Country Australia => new("Australia", "AU");
    }
}
