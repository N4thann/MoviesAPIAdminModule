using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Movie
    {
        /// <summary>
        /// Lista de URLs das imagens associadas ao filme.
        /// </summary>
        public virtual ICollection<MovieImage> Images { get; set; }

        /// <summary>
        /// Coleção de prêmios recebidos pelo filme.
        /// </summary>
        public virtual ICollection<Award> Awards { get; set; }

        /// <summary>
        /// Sinopse do filme, contendo um resumo da história.
        /// </summary>
        public string Synopsis { get; set; }

        public MovieImage? Poster { get; private set; }

        /// <summary>
        /// Ano de lançamento do filme.
        /// </summary>
        public int ReleaseYear { get; set; }

        /// <summary>
        /// Duração do filme em minutos.
        /// </summary>
        public int Duration { get; set; }

        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
    }
}
