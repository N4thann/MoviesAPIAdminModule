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
    }
}
