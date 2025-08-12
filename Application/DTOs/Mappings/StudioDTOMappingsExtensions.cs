using Application.DTOs.Response;
using Domain.Entities;
using Pandorax.PagedList;

namespace Application.DTOs.Mappings
{
    public static class StudioDTOMappingsExtensions
    {
        public static StudioInfoResponse? ToStudioDTO(this Studio studio)
        {
            if (studio == null) throw new InvalidOperationException($"Cannot map a null {typeof(Studio)} entity to {typeof(StudioInfoResponse)}. The provided 'director' object is null.");

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

        public static IPagedList<StudioInfoResponse> ToStudioPagedListDTO(this IPagedList<Studio> studiosPagedList)
        {
            if (studiosPagedList == null)
                throw new InvalidOperationException("Cannot map a null PagedList<Studio> to PagedList<StudioInfoResponse>. The provided 'studiosPagedList' object is null.");

            var studiosInfoResponses = studiosPagedList.Select(d => d.ToStudioDTO()).ToList();

            return new PagedList<StudioInfoResponse>(
                studiosInfoResponses!, 
                studiosPagedList.PageIndex,
                studiosPagedList.PageSize,
                studiosPagedList.TotalItemCount
            );
        }
    }
}
