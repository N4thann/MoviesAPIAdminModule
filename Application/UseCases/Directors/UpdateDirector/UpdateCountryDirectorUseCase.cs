using Application.DTOs.Mappings;
using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Directors.UpdateDirector
{
    public class UpdateCountryDirectorUseCase : ICommandHandler<UpdateCountryDirectorCommand, DirectorInfoResponse>
    {
        private readonly IRepository<Director> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCountryDirectorUseCase(IRepository<Director> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DirectorInfoResponse> Handle(UpdateCountryDirectorCommand command, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(command.Id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {command.Id} not found.");

            try
            {
                var country = new Country(command.CountryName, command.CountryCode);
                director.UpdateCountry(country);
                await _unitOfWork.Commit(cancellationToken);

                var response = director.ToDirectorDTO();

                return response;
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException($"An unexpected error occurred while updating Director with ID {command.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
