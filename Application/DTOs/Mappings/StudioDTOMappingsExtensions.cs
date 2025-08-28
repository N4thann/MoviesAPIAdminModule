using Application.DTOs.Response;
using Domain.Entities;
using Domain.SeedWork.Core;
using Pandorax.PagedList;

namespace Application.DTOs.Mappings
{
    public static class StudioDTOMappingsExtensions
    {
        public static Result<StudioInfoResponse>? ToStudioDTO(this Studio studio)
        {
            if (studio == null)
            {
                return Result<StudioInfoResponse>.AsFailure(
                    Failure.InfrastructureError("Attempted to map a null Studio entity. This indicates an unexpected error in the application logic."));
            }

            var response = new StudioInfoResponse(
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

            return Result<StudioInfoResponse>.AsSuccess(response);
        }

        public static Result<IPagedList<StudioInfoResponse>> ToStudioPagedListDTO(this IPagedList<Studio> studiosPagedList)
        {
            if (studiosPagedList == null) {
                return Result<IPagedList<StudioInfoResponse>>.AsFailure(
                    Failure.InfrastructureError("Cannot map a null PagedList<Studio> to PagedList<StudioInfoResponse>. The provided 'studiosPagedList' object is null."));
            }

            var studiosInfoResponses = new List<StudioInfoResponse>();

            foreach (var studio in studiosPagedList)
            {
                var mappingResult = studio.ToStudioDTO();
                if (mappingResult.IsFailure)
                {
                    return Result<IPagedList<StudioInfoResponse>>.AsFailure(mappingResult.Failure!);
                }
                studiosInfoResponses.Add(mappingResult.Success!);
            }

            var response = new PagedList<StudioInfoResponse>(
                studiosInfoResponses!, 
                studiosPagedList.PageIndex,
                studiosPagedList.PageSize,
                studiosPagedList.TotalItemCount
            );
            return Result<IPagedList<StudioInfoResponse>>.AsSuccess(response);
        }
    }
}
