namespace Domain.SeedWork.Validation
{
    public class ValidationException : Exception
    {
        public string PropertyName { get; }

        public ValidationException(string propertyName, string message)
            : base(message)
        {
            PropertyName = propertyName;
        }

        public ValidationException(string propertyName, string message, Exception innerException)
            : base(message, innerException)
        {
            PropertyName = propertyName;
        }
    }
}
