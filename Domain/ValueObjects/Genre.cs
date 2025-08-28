﻿using Domain.Entities;
using Domain.SeedWork;
using Domain.SeedWork.Core;
using Domain.SeedWork.Validation;

namespace Domain.ValueObjects
{
    public class Genre : ValueObject
    {
        public Genre() { }

        public Genre(string name, string description)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.MaxLength(name, 50, nameof(name));

            if (!string.IsNullOrWhiteSpace(description))
            {
                Validate.MaxLength(description, 500, nameof(description));
            }

            Name = name.Trim();
            Description = description;
        }

        public static Result<Genre> Create(string name, string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                var validationResult2 = Validate.MaxLength(description, 500, nameof(description));
                if (validationResult2.IsFailure)
                    return Result<Genre>.AsFailure(validationResult2.Failure!);
            }

            var validationResult = Validate.NotNullOrEmpty(name, nameof(name))
                .Combine(
                Validate.NotNullOrEmpty(description, nameof(description)),
                Validate.MaxLength(name, 50, nameof(name)));

            if (validationResult.IsFailure)
                return Result<Genre>.AsFailure(validationResult.Failure!);

            var genre = new Genre(name, description);

            return Result<Genre>.AsSuccess(genre);
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name.ToLowerInvariant();
        }

        public override string ToString() => $"Name: {Name} / Description: {Description}";

        // Factory methods
        public static Genre Action => new("Action", "Fast-paced films with physical stunts and challenges");
        public static Genre Drama => new("Drama", "Character-driven stories with emotional themes");
        public static Genre Comedy => new("Comedy", "Humorous films intended to entertain and amuse");
        public static Genre Horror => new("Horror", "Films intended to frighten and create suspense");
        public static Genre Romance => new("Romance", "Films focused on love stories and relationships");
        public static Genre SciFi => new("Science Fiction", "Films featuring futuristic or technological themes");
        public static Genre Fantasy => new("Fantasy", "Films with magical or supernatural elements");
        public static Genre Thriller => new("Thriller", "Suspenseful films with tension and excitement");
        public static Genre Animation => new("Animation", "Films created using animated techniques");
        public static Genre Documentary => new("Documentary", "Non-fiction films presenting factual content");
    }
}
