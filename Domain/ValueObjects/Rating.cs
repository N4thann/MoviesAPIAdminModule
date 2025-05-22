using Domain.SeedWork;
using Domain.SeedWork.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    /// <summary>
    /// ValueObject que representa uma avaliação de filme
    /// </summary>
    public class Rating : ValueObject
    {
        public Rating() { }

        public Rating(decimal totalSum, int votesCount, decimal maxValue = 10)
        {
            if (maxValue <= 0)
                throw new ArgumentException("Max value must be greater than 0", nameof(maxValue));

            if (totalSum < 0)
                throw new ArgumentException("Total sum cannot be negative", nameof(totalSum));

            // Se tem votos, a soma deve ser coerente com a escala
            if (votesCount > 0 && totalSum > (votesCount * maxValue))
                throw new ArgumentException("Total sum exceeds maximum possible value for given votes count");

            TotalSum = totalSum;
            VotesCount = votesCount;
            MaxValue = maxValue;
        }

        public decimal TotalSum { get; }
        public int VotesCount { get; }
        public decimal MaxValue { get; }

        // Propriedades calculadas
        public decimal AverageValue => HasVotes ? TotalSum / VotesCount : 0;
        public bool HasVotes => VotesCount > 0;

        /// <summary>
        /// Normaliza a nota para uma escala de 0 a 1 (para comparações entre diferentes escalas)
        /// </summary>
        public decimal NormalizedScore => HasVotes ? AverageValue / MaxValue : 0;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TotalSum;
            yield return VotesCount;
        }

        public override string ToString()
        {
            if (!HasVotes)
                return "No ratings";

            return $"{AverageValue:F1}/{MaxValue} ({VotesCount} votes)";
        }

        /// <summary>
        /// Cria um Rating inicial (sem votos)
        /// </summary>
        public static Rating CreateEmpty(decimal maxValue = 10)
        {
            return new Rating(0, 0, maxValue);
        }

        /// <summary>
        /// Adiciona um novo voto ao rating atual
        /// </summary>
        public Rating AddVote(decimal voteValue)
        {
            if (voteValue < 0 || voteValue > MaxValue)
                throw new ArgumentException($"Vote value must be between 0 and {MaxValue}", nameof(voteValue));

            var newTotalSum = TotalSum + voteValue;
            var newVotesCount = VotesCount + 1;

            return new Rating(newTotalSum, newVotesCount, MaxValue);
        }

        /// <summary>
        /// Remove um voto do rating atual (caso necessário para correções)
        /// </summary>
        public Rating RemoveVote(decimal voteValue)
        {
            if (VotesCount == 0)
                throw new InvalidOperationException("Cannot remove vote from rating with no votes");

            if (voteValue < 0 || voteValue > MaxValue)
                throw new ArgumentException($"Vote value must be between 0 and {MaxValue}", nameof(voteValue));

            var newTotalSum = TotalSum - voteValue;
            var newVotesCount = VotesCount - 1;

            // Se ficou sem votos, retorna rating vazio
            if (newVotesCount == 0)
                return CreateEmpty(MaxValue);

            return new Rating(newTotalSum, newVotesCount, MaxValue);
        }
    }
}
