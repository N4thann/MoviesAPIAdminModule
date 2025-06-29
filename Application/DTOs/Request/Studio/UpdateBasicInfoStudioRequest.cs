using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Studio
{
    public record class UpdateBasicInfoStudioRequest(
        string Name,
        string History
        );
}
