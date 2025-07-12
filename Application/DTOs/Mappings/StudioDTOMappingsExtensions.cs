using Application.Common;
using Application.DTOs.Response.Director;
using Application.DTOs.Response.Studio;
using Domain.Entities;

namespace Application.DTOs.Mappings
{
    public static class StudioDTOMappingsExtensions
    {
        public static StudioInfoResponse? ToStudioDTO(this Studio studio)
        {
            if (studio == null) throw new InvalidOperationException(nameof(studio));

            return new StudioInfoResponse(
                    studio.Id,
                    studio.Name,
                    studio.Country.Name,
                    studio.Country.Code,
                    studio.FoundationDate,
                    studio.History,
                    studio.IsActive,
                    studio.CreatedAt,
                    studio.UpdatedAt,
                    studio.YearsInOperation
                );
        }

        public static PagedList<StudioInfoResponse> ToStudioPagedListDTO(this PagedList<Studio> studiosPagedList)
        {
            if (studiosPagedList == null)
                throw new InvalidOperationException("Cannot map a null PagedList<Studio> to PagedList<StudioInfoResponse>. The provided 'studiosPagedList' object is null.");

            var studiosInfoResponses = studiosPagedList.Select(d => d.ToStudioDTO()).ToList();

            return new PagedList<StudioInfoResponse>(
                studiosInfoResponses!, // O ToStudioDTO pode retornar null, então use ! se tiver certeza que não será null ou adicione tratamento
                studiosPagedList.TotalCount,
                studiosPagedList.CurrentPage,
                studiosPagedList.PageSize
            );
        }
    }
}
