using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Queries.User
{
    public record GetAllUsersQuery() : IQuery<Result<IEnumerable<UserSummaryResponse>>>;
}
