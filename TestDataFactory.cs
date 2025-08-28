using Domain.Entities;
using Domain.ValueObjects;

namespace Tests.Common
{
    public static class TestDataFactory
    {
        // Fábrica para um Diretor válido
        public static Director CreateFakeDirector()
        {
            var director = new Director("Christopher Nolan", new DateTime(1970, 7, 30), new Country("UK", "GB"));
            // Em testes, às vezes precisamos setar o Id manualmente para simular uma entidade que já existe.
            // Para isso, o set do Id na entidade precisaria ser 'internal' ou 'public', ou usar reflexão.
            // Por simplicidade, vamos assumir que o Id pode ser setado.
            // PropertyInfo propertyInfo = typeof(BaseEntity).GetProperty("Id");
            // propertyInfo.SetValue(director, Guid.NewGuid());
            return director;
        }

        public static Studio CreateFakeStudio()
        {
            var studio = new Studio("Warner Bros.", "USA");
            // ... setar o Id se necessário
            return studio;
        }

        // Fábrica para um Filme válido, que depende de outros objetos
        public static Movie CreateFakeMovie()
        {
            var director = CreateFakeDirector();
            var studio = CreateFakeStudio();

            return new Movie(
                "Inception",
                "Inception",
                "A mind-bending thriller.",
                2010,
                new Duration(148),
                new Country("USA", "US"),
                studio,
                director,
                new Genre("Sci-Fi", "Science Fiction"),
                new Money(829000000, "USD"),
                new Money(160000000, "USD")
            );
        }

        // Fábrica para um Command válido
        public static CreateMovieCommand CreateFakeCreateMovieCommand(Guid directorId, Guid studioId)
        {
            return new CreateMovieCommand(
                "Inception", "Inception", "A mind-bending thriller.", 2010,
                148, "USA", "US", "Sci-Fi", "...", 829, "USD", 160, "USD",
                directorId, studioId
            );
        }
    }
}