using Domain.Enums;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Mvc;

namespace MoviesAPIAdminModule.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult HandleFailure(Failure failure)
        {
            return failure.Type switch
            {
                FailureType.NotFound => NotFound(failure),
                FailureType.Validation => BadRequest(failure),
                FailureType.Unauthorized => Unauthorized(failure),
                FailureType.Forbidden => Forbid(), // Forbid não leva um corpo
                FailureType.Conflict => Conflict(failure),
                _ => StatusCode(500, failure) // InternalServer e Infrastructure
            };
        }
    }
}
