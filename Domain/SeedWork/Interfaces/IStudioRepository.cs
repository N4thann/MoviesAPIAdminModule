using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SeedWork.Interfaces
{
    public interface IStudioRepository 
    {
        Task AddAsync(Studio studio);

        Task<Studio?> GetByIdAsync(Guid id);

        Task<IEnumerable<Studio>> GetAllAsync();
    }
}
