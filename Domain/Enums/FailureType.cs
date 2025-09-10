namespace Domain.Enums
{
    public enum FailureType
    {
        Validation,         // 400
        Unauthorized,       // 401
        Forbidden,          // 403
        NotFound,           // 404
        Conflict,           // 409
        InternalServer,     // 500 (Bugs, exceções inesperadas)
        Infrastructure      // 500 (Falhas em serviços externos: DB, S3, etc.)
    }
}
