using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public record class StudioInfoResponse(
        Guid Id,
        string Name,
        string CountryName,
        string CountryCode,
        DateTime FoundationDate,
        string? History,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        int YearsInOperation
        );
}
