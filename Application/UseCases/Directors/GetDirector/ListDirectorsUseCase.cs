using Application.DTOs.Response.Director;
using Application.Interfaces;
using Application.UseCases.Studios.GetStudio;
using Domain.SeedWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Directors.GetDirector
{
    public  class ListDirectorsUseCase : IQueryHandler<ListDirectorsQuery, IEnumerable<DirectorInfoResponse>>
    {
        private readonly IDirectorRepository _repository;

        public ListDirectorsUseCase(IDirectorRepository repository) => _repository = repository;

        public async Task<IEnumerable<DirectorInfoResponse>> Handle(ListDirectorsQuery query, CancellationToken cancellationToken)
        {
            var directors = await _repository.GetAllAsync();

            var response = directors.Select(director => new DirectorInfoResponse(
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
                )).ToList();

                return response;
        }
    }
}
