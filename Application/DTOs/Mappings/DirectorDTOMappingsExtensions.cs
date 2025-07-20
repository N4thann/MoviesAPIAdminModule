using Application.Common;
using Application.DTOs.Response;
using Domain.Entities;
using X.PagedList;

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

        public static IPagedList<DirectorInfoResponse> ToDirectorPagedListDTO(this IPagedList<Director> directorsPagedList)
        {
            if (directorsPagedList == null)
                throw new InvalidOperationException("Cannot map a null PagedList<Director> to PagedList<DirectorInfoResponse>. The provided 'directorsPagedList' object is null.");

            var directorInfoResponses = directorsPagedList.Select(d => d.ToDirectorDTO()).ToList();

            // Usa StaticPagedList para criar um IPagedList<DirectorInfoResponse>
            // Ele recria a lista paginada com os metadados existentes e os novos itens mapeados.
            return new StaticPagedList<DirectorInfoResponse>(
                directorInfoResponses!,
                directorsPagedList.PageNumber,
                directorsPagedList.PageSize,
                directorsPagedList.TotalItemCount
            );
        }
    }
}
