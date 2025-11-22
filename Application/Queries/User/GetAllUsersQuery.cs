using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Domain.SeedWork.Core;

namespace Application.Queries.User
{
    public record GetAllUsersQuery() : IQuery<Result<IEnumerable<UserSummaryResponse>>>;
}
