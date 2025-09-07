using Application.DTOs.Authentication;
using Domain.SeedWork.Core;

namespace Application.Interfaces
{
    /// <summary>
    /// Define um contrato para operações de acesso a dados de usuários,
    /// abstraindo a implementação concreta do ASP.NET Core Identity.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Encontra um usuário pelo seu nome de usuário.
        /// </summary>
        /// <param name="username">O nome de usuário a ser procurado.</param>
        /// <returns>Um objeto simples representando o usuário ou nulo se não for encontrado.</returns>
        Task<UserDto?> FindByNameAsync(string username);

        /// <summary>
        /// Verifica se a senha fornecida corresponde à senha do usuário.
        /// </summary>
        /// <param name="user">O usuário para o qual a senha será verificada.</param>
        /// <param name="password">A senha a ser verificada.</param>
        /// <returns>'true' se a senha for válida, caso contrário 'false'.</returns>
        Task<bool> CheckPasswordAsync(UserDto user, string password);

        /// <summary>
        /// Obtém a lista de roles (funções) de um usuário.
        /// </summary>
        /// <param name="user">O usuário cujas roles serão obtidas.</param>
        /// <returns>Uma lista de strings contendo os nomes das roles.</returns>
        Task<IList<string>> GetRolesAsync(UserDto user);

        /// <summary>
        /// Atualiza o Refresh Token e sua data de expiração para um usuário.
        /// </summary>
        /// <param name="userId">O ID do usuário a ser atualizado.</param>
        /// <param name="refreshToken">O novo refresh token.</param>
        /// <param name="expiryTime">A nova data de expiração do token.</param>
        /// <returns>Um Result indicando sucesso ou falha na operação.</returns>
        Task<Result<bool>> UpdateUserRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime);
    }
}
