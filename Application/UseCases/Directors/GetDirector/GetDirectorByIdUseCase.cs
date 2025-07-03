using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors.GetDirector
{
    public class GetDirectorByIdUseCase : IQueryHandler<GetDirectorByIdQuery, DirectorInfoResponse>
    {
        private readonly IRepository<Director> _repository;

        public GetDirectorByIdUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<DirectorInfoResponse> Handle(GetDirectorByIdQuery query, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(query.Id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {query.Id} not found.");
            try
            {
                var response = new DirectorInfoResponse(
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
                    );

                return response;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred while updating Director with ID {query.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
