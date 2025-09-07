using System.Text.Json.Serialization;

namespace Application.DTOs.Response
{
    /// <summary>
    /// Representa a resposta enviada ao cliente após um login bem-sucedido.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// O token de acesso (JWT) de curta duração.
        /// </summary>
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// O token de atualização de longa duração, usado para obter um novo token de acesso.
        /// </summary>
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// A data e hora exatas em que o AccessToken irá expirar.
        /// </summary>
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }
    }
}
