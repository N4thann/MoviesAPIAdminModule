﻿using Application.Commands.Director;
using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Directors
{
    public class CreateDirectorUseCase : ICommandHandler<CreateDirectorCommand, Result<DirectorInfoResponse>>
    {
        private readonly IRepository<Director> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDirectorUseCase(IRepository<Director> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DirectorInfoResponse>> Handle(CreateDirectorCommand command, CancellationToken cancellationToken)
        {
            var countryResult = Country.Create(command.CountryName, command.CountryCode);

            if (countryResult.IsFailure)
                return Result<DirectorInfoResponse>.AsFailure(countryResult.Failure!);

            var directorResult = Director.Create(
                command.Name,
                command.BirthDate,
                countryResult.Success!,
                command.Biography,
                command.Gender
                );

            if (directorResult.IsFailure)
                return Result<DirectorInfoResponse>.AsFailure(directorResult.Failure!);

            var director = directorResult.Success!;

            _repository.Add(director!);
            await _unitOfWork.Commit(cancellationToken);

            return director.ToDirectorDTO();
        }
    }
}
