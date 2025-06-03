namespace Domain.SeedWork.Validation
{
    public static class Validate
    {
        public static void NotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException(parameterName, $"{parameterName} cannot be null or empty.");
            }
        }

        public static void NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ValidationException(parameterName, $"{parameterName} cannot be null.");
            }
        }

        public static void IsPastDate(DateTime date, string parameterName, bool allowToday = false)
        {
            DateTime now = DateTime.UtcNow.Date;
            if (allowToday)
            {
                if (date.Date > now)
                {
                    throw new ValidationException(parameterName, $"{parameterName} cannot be a future date.");
                }
            }
            else
            {
                if (date.Date >= now)
                {
                    throw new ValidationException(parameterName, $"{parameterName} must be a past date.");
                }
            }
        }

        public static void GreaterThan(int value, int minimum, string parameterName)
        {
            if (value <= minimum)
            {
                throw new ValidationException(parameterName, $"{parameterName} must be greater than {minimum}. Current value: {value}");
            }
        }

        public static void LessThanOrEqualTo(int value, int maximum, string parameterName)
        {
            if (value > maximum)
            {
                throw new ValidationException(parameterName, $"{parameterName} must be less than or equal to {maximum}. Current value: {value}");
            }
        }

        public static void Range(int value, int minimum, int maximum, string parameterName)
        {
            if (value < minimum || value > maximum)
            {
                throw new ValidationException(parameterName,
                    $"{parameterName} must be between {minimum} and {maximum}. Current value: {value}");
            }
        }

        public static void MinLength(string value, int minLength, string parameterName)
        {
            if (value != null && value.Length < minLength)
            {
                throw new ValidationException(parameterName,
                    $"{parameterName} must be at least {minLength} characters long. Current length: {value.Length}");
            }
        }

        public static void MaxLength(string value, int maxLength, string parameterName)
        {
            if (value != null && value.Length > maxLength)
            {
                throw new ValidationException(parameterName,
                    $"{parameterName} must not exceed {maxLength} characters. Current length: {value.Length}");
            }
        }

        public static void Email(string value, string parameterName)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, emailRegex))
                {
                    throw new ValidationException(parameterName, $"{parameterName} must be a valid email address.");
                }
            }
        }

        public static void Matches(string value, string pattern, string parameterName)
        {
            if (!string.IsNullOrWhiteSpace(value) && !System.Text.RegularExpressions.Regex.IsMatch(value, pattern))
            {
                throw new ValidationException(parameterName, $"{parameterName} does not match the required pattern.");
            }
        }

        public static void IsValidUrl(string value, string name)
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out Uri uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException($"{name} must be a valid HTTP or HTTPS URL.", name);
            }
        }
    }
}
