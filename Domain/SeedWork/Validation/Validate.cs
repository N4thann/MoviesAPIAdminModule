using Domain.SeedWork.Core;

namespace Domain.SeedWork.Validation
{
    public static class Validate
    {
        public static Result<bool> NotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<bool>.AsFailure(Failure.Validation($"{parameterName} cannot be null or empty."));
            else
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
                return Result<bool>.AsFailure(Failure.Validation($"{parameterName} cannot be null or empty."));
            else
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> IsPastDate(DateTime date, string parameterName, bool allowToday = false)
        {
            DateTime now = DateTime.UtcNow.Date;
            if (allowToday)
            {
                if (date.Date > now)
                    return Result<bool>.AsFailure(Failure.Validation($"{parameterName} cannot be a future date."));
                else
                    return Result<bool>.AsSuccess(true);
                }
            else
            {
                if (date.Date >= now)
                    return Result<bool>.AsFailure(Failure.Validation($"{parameterName} must be a past date."));
                else
                    return Result<bool>.AsSuccess(true);
            }
        }

        public static Result<bool> GreaterThan(int value, int minimum, string parameterName)
        {
            if (value <= minimum)
                return Result<bool>.AsFailure(Failure.Validation($"{parameterName} must be greater than {minimum}. Current value: {value}"));
            else
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> LessThanOrEqualTo(int value, int maximum, string parameterName)
        {
            if (value > maximum)
                return Result<bool>.AsFailure(Failure.Validation($"{parameterName} must be less than or equal to {maximum}. Current value: {value}"));
            else
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> Range(int value, int minimum, int maximum, string parameterName)
        {
            if (value < minimum || value > maximum)
                return Result<bool>.AsFailure(Failure.Validation($"{parameterName} must be between {minimum} and {maximum}. Current value: {value}"));
            else 
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> MinLength(string value, int minLength, string parameterName)
        {
            if (value != null && value.Length < minLength)
                return Result<bool>.AsFailure(Failure.Validation($"{parameterName} must be at least {minLength} characters long. Current length: {value.Length}"));
            else 
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> MaxLength(string value, int maxLength, string parameterName)
        {
            if (value != null && value.Length > maxLength)
                return Result<bool>.AsFailure(Failure.Validation($"{parameterName} must not exceed {maxLength} characters. Current length: {value.Length}"));
            else
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> Email(string value, string parameterName)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return Result<bool>.AsFailure(Failure.Validation($"{value} cannot be null or empty."));

            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(value, emailRegex))
               return Result<bool>.AsFailure(Failure.Validation($"{parameterName} must be a valid email address."));
            else
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> Matches(string value, string pattern, string parameterName)
        {
            if (!string.IsNullOrWhiteSpace(value) && !System.Text.RegularExpressions.Regex.IsMatch(value, pattern))
                return Result<bool>.AsFailure(Failure.Validation($"{parameterName} does not match the required pattern."));
            else
                return Result<bool>.AsSuccess(true);
        }

        public static Result<bool> IsValidUrl(string value, string name)
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out Uri uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                return Result<bool>.AsFailure(Failure.Validation($"{name} must be a valid HTTP or HTTPS URL."));
            else
                return Result<bool>.AsSuccess(true);
        }
    }
}
