namespace Domain.SeedWork.Core
{
    public sealed record Failure(int Code, string Message)
    {
        public static readonly Failure InternalServerError = new(500, "An unexpected internal server error occurred.");
        public static readonly Failure ValidationError = new(400, "One or more validation errors occurred.");

        public static readonly Failure Unauthorized = new(401, "Authentication failed. Please provide a valid token.");
        public static readonly Failure InvalidCredentials = new(401, "Invalid credentials provided.");
        public static readonly Failure TokenExpired = new(401, "The provided token has expired.");
        public static readonly Failure Forbidden = new(403, "You do not have permission to access this resource.");

        public static Failure InfrastructureError(string message) => new(500, message);
        public static Failure Validation(string message) => new(400, message);
        public static Failure NotFound(string entityName, object key) => new(404, $"The {entityName} with key '{key}' was not found.");
        public static Failure Conflict(string message) => new(409, message);
    }
}
