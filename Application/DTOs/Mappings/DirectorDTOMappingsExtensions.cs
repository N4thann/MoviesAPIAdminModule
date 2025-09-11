using Application.DTOs.Response;
using Domain.Entities;
using Domain.SeedWork.Core;
using Pandorax.PagedList;

namespace Application.DTOs.Mappings
{
    public static class DirectorDTOMappingsExtensions
    {
        public static Result<DirectorInfoResponse>? ToDirectorDTO(this Director director)
        {
            if (director == null)
            {
                return Result<DirectorInfoResponse>.AsFailure(
                    Failure.Infrastructure("Attempted to map a null Director entity. This indicates an unexpected error in the application logic."));
            }

            var response = new DirectorInfoResponse(
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
            return Result<DirectorInfoResponse>.AsSuccess(response);
        }

        public static Result<IPagedList<DirectorInfoResponse>> ToDirectorPagedListDTO(this IPagedList<Director> directorsPagedList)
        {
            if (directorsPagedList == null)
            {
                return Result<IPagedList<DirectorInfoResponse>>.AsFailure(
                    Failure.Infrastructure("Cannot map a null PagedList<Director> to PagedList<DirectorInfoResponse>. The provided 'directorsPagedList' object is null."));
            }

            var directorInfoResponses = new List<DirectorInfoResponse>();

            foreach (var director in directorsPagedList)
            {
                var mappingResult = director.ToDirectorDTO();

                if (mappingResult.IsFailure)
                {
                    return Result<IPagedList<DirectorInfoResponse>>.AsFailure(mappingResult.Failure!);
                }
                directorInfoResponses.Add(mappingResult.Success!);
            }

            var response = new PagedList<DirectorInfoResponse>(
                directorInfoResponses!,
                directorsPagedList.PageIndex,
                directorsPagedList.PageSize,
                directorsPagedList.TotalItemCount
            );

            return Result<IPagedList<DirectorInfoResponse>>.AsSuccess(response);
        }
    }
}
