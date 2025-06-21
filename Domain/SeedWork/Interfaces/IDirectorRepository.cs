using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SeedWork.Interfaces
{
    public interface IDirectorRepository
    {
        Task AddAsync(Director director);
    }
}
