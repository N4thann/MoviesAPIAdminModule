using Application.DTOs.Response.Director;
using Domain.Entities;

namespace Application.DTOs.Mappings
{
    public static class DirectorDTOMappingsExtensions
    {
        public static DirectorInfoResponse? ToDirectorDTO(this Director director)
        {
            if (director == null) throw new InvalidOperationException(nameof(director));

            return new DirectorInfoResponse(
                    director.Id,
                    director.Name,
                    director.BirthDate,
                    director.Country.Name,
                    director.Country.Code,
                    director.Biography,
                    director.IsActive,
                    director.CreatedAt,
                    director.UpdatedAt,
                    director.Age
                    );
        }
    }
}
