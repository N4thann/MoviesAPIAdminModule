using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Common
{
    public record class UpdateCountryRequest(
        string CountryName,
        string CountryCode
        );
}
