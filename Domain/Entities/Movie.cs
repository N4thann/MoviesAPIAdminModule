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
        public virtual ICollection<MovieImage> Images { get; set; }

        public virtual ICollection<Award> Awards { get; set; }

        public string Synopsis { get; set; }

        public MovieImage? Poster { get; private set; }

        public int ReleaseYear { get; set; }

        public Duration Duration { get; set; }


        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
    }
}
