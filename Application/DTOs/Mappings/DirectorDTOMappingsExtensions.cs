using Application.Common;
using Application.DTOs.Response.Director;
using Domain.Entities;

namespace Application.DTOs.Mappings
{
    public static class DirectorDTOMappingsExtensions
    {
        public static DirectorInfoResponse? ToDirectorDTO(this Director director)
        {
            if (director == null) throw new InvalidOperationException("Cannot map a null Director entity to DirectorInfoResponse. The provided 'director' object is null.");

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

        public static PagedList<DirectorInfoResponse> ToDirectorPagedListDTO(this PagedList<Director> directorsPagedList)
        {
            if (directorsPagedList == null)
                throw new InvalidOperationException("Cannot map a null PagedList<Director> to PagedList<DirectorInfoResponse>. The provided 'directorsPagedList' object is null.");

            var directorInfoResponses = directorsPagedList.Select(d => d.ToDirectorDTO()).ToList();

            return new PagedList<DirectorInfoResponse>(
                directorInfoResponses!, // O ToDirectorDTO pode retornar null, então use ! se tiver certeza que não será null ou adicione tratamento
                directorsPagedList.TotalCount,
                directorsPagedList.CurrentPage,
                directorsPagedList.PageSize
            );
        }
    }
}
