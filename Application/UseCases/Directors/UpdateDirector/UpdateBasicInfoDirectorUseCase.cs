using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors.UpdateDirector
{
    public class UpdateBasicInfoDirectorUseCase : ICommandHandler<UpdateBasicInfoDirectorCommand, DirectorInfoResponse>
    {
        private readonly IRepository<Director> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBasicInfoDirectorUseCase(IRepository<Director> repositoty, IUnitOfWork unitOfWork)
        {
            _repository = repositoty;
            _unitOfWork = unitOfWork;
        }

        public async Task<DirectorInfoResponse> Handle(UpdateBasicInfoDirectorCommand command, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(command.Id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {command.Id} not found.");
            try
            {
                director.UpdateBasicInfo(command.Name, command.NewBirthDate, command.Gender, command.Biography);
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
            catch (Exception ex) 
            {
                throw new InvalidOperationException($"An unexpected error occurred while updating Director with ID {command.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
