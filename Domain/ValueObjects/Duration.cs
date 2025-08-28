using Domain.SeedWork;
using Domain.SeedWork.Core;
using Domain.SeedWork.Validation;

namespace Domain.ValueObjects
{
    public class Duration : ValueObject
    {
        private const int MIN_DURATION_MINUTES = 1;
        private const int MAX_DURATION_MINUTES = 600;

        public Duration() { }

        private Duration(int minutes) 
        {
           Minutes = minutes;   
        }

        public static Result<Duration> Create(int minutes)
        {
            var validationResult = Validate.Range(minutes, MIN_DURATION_MINUTES, MAX_DURATION_MINUTES, nameof(minutes))
                .Combine(
                    Validate.LessThanOrEqualTo(minutes, MAX_DURATION_MINUTES, nameof(minutes)));

            if (validationResult.IsFailure)
                return Result<Duration>.AsFailure(validationResult.Failure!);

             var duration = new Duration(minutes);

            return Result<Duration>.AsSuccess(duration);
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
    }
}
