namespace Application.DTOs.Authentication
{
    /// <summary>
    /// Um DTO (Data Transfer Object) simples para transportar informações básicas do usuário
    /// entre a camada de Infraestrutura e a de Aplicação, sem expor o IdentityUser.
    /// </summary>
    public class UserDto
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
