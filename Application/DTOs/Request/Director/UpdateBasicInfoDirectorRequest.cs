using Domain.Entities;

namespace Application.DTOs.Request.Director
{
    public record class UpdateBasicInfoDirectorRequest(
        string Name,
        DateTime NewBirthDate,
        Gender Gender,
        string? Biography
        );
}
