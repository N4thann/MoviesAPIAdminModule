using Application.DTOs.Mappings;
using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.SeedWork.Validation;
using Domain.ValueObjects;

namespace Application.UseCases.Directors.CreateDirector
{
    public class CreateDirectorUseCase : ICommandHandler<CreateDirectorCommand, DirectorInfoResponse>
    {
        private readonly IRepository<Director> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDirectorUseCase(IRepository<Director> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DirectorInfoResponse> Handle(CreateDirectorCommand command, CancellationToken cancellationToken)
        {
            var country = new Country(command.CountryName, command.CountryCode);

            try
            {
                var director = new Director(
                command.Name,
                command.BirthDate,
                country,
                command.Biography,
                command.Gender
                );

                await _repository.AddAsync(director);
                await _unitOfWork.Commit(cancellationToken);

                var response = director.ToDirectorDTO();

                return response;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }
    }
}
