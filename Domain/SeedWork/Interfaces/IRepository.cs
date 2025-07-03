using System.Linq.Expressions;

namespace Domain.SeedWork.Interfaces
{
    /// <summary>
    /// Interface genérica para repositórios, fornecendo operações básicas de CRUD e consultas.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtém uma entidade pelo ID.
        /// </summary>
        /// <param name="id">Identificador da entidade.</param>
        /// <returns>Entidade correspondente ao ID informado.</returns>
        Task<T> GetByIdAsync(Guid id);
        /// <summary>
        /// Obtém todas as entidades do repositório.
        /// </summary>
        /// <returns>Lista de entidades.</returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Obtém um IQueryable para realizar consultas avançadas sobre as entidades.
        /// </summary>
        /// <returns>Consulta IQueryable.</returns>
        IQueryable<T> GetAllQueryable();
        /// <summary>
        /// Verifica se existe alguma entidade que atende ao critério informado.
        /// </summary>
        /// <param name="predicate">Expressão para filtrar a existência da entidade.</param>
        /// <returns>Verdadeiro se a entidade existir, falso caso contrário.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Adiciona uma nova entidade ao repositório. Método para o UnitOfWork
        /// </summary>
        /// <param name="entity">Entidade a ser adicionada.</param>
        Task AddAsync(T entity);
        /// <summary>
        /// Adiciona uma coleção de entidades ao banco de dados de forma assíncrona. Método para o UnitOfWork
        /// </summary>
        /// <param name="entities">Lista de entidades a serem adicionadas.</param>
        /// <returns>Uma tarefa assíncrona que representa a operação.</returns>
        /// <remarks>
        /// Este método melhora a performance ao inserir múltiplas entidades em uma única transação,
        /// reduzindo o número de chamadas ao banco de dados.
        /// </remarks>
        Task AddRangeAsync(IEnumerable<T> entities);
        /// <summary>
        /// Remove uma entidade do repositório com base no ID informado. Método para o UnitOfWork
        /// </summary>
        /// <param name="entity">Identificador da entidade a ser removida.</param>
        public void Delete(T entity);
    }
}
// Mesmo que esse método GetAllAsync() seja para uma listagem em uma tabela em que cada registro tenha uma opção
// de edição, o AsNoTracking ainda irá otimizar apenas para o carregamento da tabela 
// e posteriormente posso fazer uma procura sem o AsNoTracking para procurar o registro com GetByIdAsync       

/* Por que GetAllQueryable não precisa ser assíncrono?
IQueryable representa apenas a definição da consulta e não a execução dela.
Ele não faz nenhuma operação no banco até que seja "materializado" (com ToList, First, etc.).
Tornar o método assíncrono criaria complexidade desnecessária sem ganho real.
O compilador não permite o uso de await para um retorno que já é materializado de forma síncrona.
 */

