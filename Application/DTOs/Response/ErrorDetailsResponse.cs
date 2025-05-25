using System.Text.Json;

namespace Application.DTOs.Response
{
    /// <summary>
    /// Essa classe representa uma resposta automática via Middleware de Exceções que podem ocorrer no sistema,
    /// devolvendo no Json detalhes da Exceção
    /// </summary>
    public class ErrorDetailsResponse
    {
        /// <summary>
        /// Representa o códico de Status Http
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Representa a mensagem de erro
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Representa o rastreamento da pilha
        /// </summary>
        public string? Trace { get; set; }

        /// <summary>
        /// Representa a serialização da instância da classe para retornar em Json 
        /// </summary>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);// Sobrescreve o método ToString
        }
    }

}
