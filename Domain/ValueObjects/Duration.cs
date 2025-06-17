using Domain.SeedWork;
using Domain.SeedWork.Validation;

namespace Domain.ValueObjects
{
    public class Duration : ValueObject
    {
        public Duration() { }

        public Duration(int minutes) 
        {
            Validate.GreaterThan(minutes, 0, nameof(minutes));
            Validate.LessThanOrEqualTo(minutes, 1000, nameof(minutes));

           Minutes = minutes;   
        }
        // Propriedade principal
        public int Minutes { get; private set; }

        // Propriedades calculadas
        public int Hours => Minutes / 60;
        public int RemainingMinutes => Minutes % 60;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Minutes;
        }

        public override string ToString() 
        {
            if (Minutes < 60)
                return $"{Minutes}min";

            return RemainingMinutes == 0
                ? $"{Hours}h"
                : $"{Hours}h {RemainingMinutes}min";
        }

        public static Duration FromHoursAndMinutes(int hours, int minutes)
        {
            Validate.GreaterThan(hours, -1, nameof(hours));
            Validate.Range(minutes, 0, 59, nameof(minutes));

            return new Duration(hours * 60 + minutes);
        }
    }
}
