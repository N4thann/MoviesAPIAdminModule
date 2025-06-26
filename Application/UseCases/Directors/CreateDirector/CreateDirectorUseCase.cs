using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Directors.CreateDirector
{
    public class CreateDirectorUseCase : ICommandHandler<CreateDirectorCommand, DirectorInfoResponse>
    {
        private readonly IDirectorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDirectorUseCase(IDirectorRepository directorRepository, IUnitOfWork unitOfWork)
        {
            _repository = directorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DirectorInfoResponse> Handle(CreateDirectorCommand command, CancellationToken cancellationToken)
        {
            var country = new Country(command.CountryName, command.CountryCode);

            var director = new Director(
                command.Name,
                command.BirthDate,
                country,
                command.Biography,
                command.Gender
                );

            await _repository.AddAsync(director);
            await _unitOfWork.Commit(cancellationToken);

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
    }
}
