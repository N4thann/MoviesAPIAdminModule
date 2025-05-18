namespace Domain.SeedWork
{
    /// <summary>
    /// Classe base abstrata para Value Objects
    /// Define comportamento padrão de igualdade e comparação para objetos de valor imutáveis
    /// </summary>
    public abstract class ValueObject//É o "coração" do sistema de comparação
    {
        /// <summary>
        /// Retorna os componentes que definem igualdade para este objeto de valor
        /// Implementação obrigatória em classes filhas para definir quais propriedades
        /// determinam se dois objetos são considerados iguais
        /// </summary>
        /// <returns>Sequência de objetos que representam as propriedades relevantes para comparação</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// Determina se o objeto especificado é igual ao objeto atual
        /// Compara os componentes de igualdade elemento por elemento
        /// </summary>
        /// <param name="obj">Objeto a ser comparado com o objeto atual</param>
        /// <returns>true se os objetos são iguais; caso contrário, false</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())//Verifica se objeto é nulo OU tipos diferentes
            {
                return false;
            }

            var valueObject = (ValueObject)obj;//Converte objeto para ValueObject (já sabemos que é do tipo correto)
            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());//Compara os componentes de igualdade elemento por elemento
        }

        /// <summary>
        /// Calcula e retorna o código hash para este objeto de valor
        /// Combina os códigos hash de todos os componentes de igualdade usando XOR
        /// Garante que objetos iguais tenham o mesmo código hash
        /// </summary>
        /// <returns>Código hash calculado a partir dos componentes de igualdade</returns>
        public override int GetHashCode()
        {
            return GetEqualityComponents()//Pega todos os componentes
                .Select(obj => obj?.GetHashCode() ?? 0)//Calcula hash de cada componente (ou 0 se nulo)
                .Aggregate((x, y) => x ^ y);//Combina todos os hashes usando XOR
        }

        /// <summary>
        /// Operador de igualdade que compara dois objetos de valor
        /// Verifica referências nulas e delega para o método Equals
        /// </summary>
        /// <param name="left">Primeiro objeto de valor a ser comparado</param>
        /// <param name="right">Segundo objeto de valor a ser comparado</param>
        /// <returns>true se os objetos são iguais; caso contrário, false</returns>
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))//Ambos nulos = iguais
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))//Um nulo = diferentes
                return false;

            return left.Equals(right);//Usa o método Equals() que implementamos acima
        }

        /// <summary>
        /// Operador de desigualdade que compara dois objetos de valor
        /// Retorna o inverso do operador de igualdade
        /// </summary>
        /// <param name="left">Primeiro objeto de valor a ser comparado</param>
        /// <param name="right">Segundo objeto de valor a ser comparado</param>
        /// <returns>true se os objetos são diferentes; caso contrário, false</returns>
        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);//Simplesmente nega o resultado do operador ==
        }

        /// <summary>
        /// Cria uma cópia do objeto de valor
        /// Como objetos de valor são imutáveis, retorna a mesma instância
        /// Classes filhas podem sobrescrever para implementar cópia personalizada
        /// </summary>
        /// <returns>Referência para o mesmo objeto (já que é imutável)</returns>
        public virtual ValueObject Copy()
        {
            return this;//Não precisa criar cópia real, se precisar de cópia "real", classes filhas podem sobrescrever
        }
    }
}
