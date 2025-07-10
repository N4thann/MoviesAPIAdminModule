using Application.DTOs.Mappings;
using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Application.UseCases.Studios.UpdateStudio;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.SeedWork.Validation;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Linq;

namespace Application.UseCases.Studios.PatchStudio
{
    public class PatchStudioCommandHandler : ICommandHandler<PatchStudioCommand, StudioInfoResponse>
    {
        private readonly IRepository<Studio> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PatchStudioCommandHandler(IRepository<Studio> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudioInfoResponse> Handle(PatchStudioCommand command, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(command.Id);

            if (studio == null)
            {
                throw new KeyNotFoundException($"Studio with ID {command.Id} not found.");
            }

            string? newName = null;
            string? newHistory = null;
            Country? newCountry = null;
            DateTime? newFoundationDate = null;
            bool? newIsActiveState = null;

            bool namePatched = false;
            bool historyPatched = false;

            foreach (var op in command.PatchDocument.Operations)
            {
                string normalizedPath = op.path.Trim('/').ToLowerInvariant();

                if (op.OperationType != OperationType.Replace)//Permitir apenas comandos replace do JsonPatch
                {
                    throw new InvalidOperationException($"Patch operation '{op.OperationType}' for path '{op.path}' is not supported.");
                }

                switch (normalizedPath)
                {
                    case "name":
                        newName = op.value?.ToString();
                        namePatched = true;
                        break;
                    case "history":
                        newHistory = op.value?.ToString();
                        historyPatched = true;
                        break;
                    case "country":
                        if (op.value is JObject countryJObject)
                        {
                            string? countryName = countryJObject["name"]?.ToString();
                            string? countryCode = countryJObject["code"]?.ToString();
                            if (string.IsNullOrEmpty(countryName) || string.IsNullOrEmpty(countryCode))
                            {
                                throw new ValidationException("country", "For country update, 'name' and 'code' are required in the country object within the patch.");
                            }
                            newCountry = new Country(countryName, countryCode);
                        }
                        else
                        {
                            throw new ValidationException("country", "Patch value for 'country' must be a JSON object with 'name' and 'code'.");
                        }
                        break;
                    case "foundationdate":
                        if (op.value is DateTime fd)
                        {
                            newFoundationDate = fd;
                        }
                        else if (op.value is string fdString && DateTime.TryParse(fdString, out var parsedFd))
                        {
                            newFoundationDate = parsedFd;
                        }
                        else
                        {
                            throw new ValidationException("foundationDate", "Patch value for 'foundationDate' must be a valid date.");
                        }
                        break;
                    case "isactive":
                        if (op.value is bool isActiveBool)
                        {
                            newIsActiveState = isActiveBool;
                        }
                        else
                        {
                            throw new ValidationException("isActive", "Patch value for 'isActive' must be a boolean (true/false).");
                        }
                        break;
                    default:
                        throw new InvalidOperationException($"Patch operation for path '{op.path}' is not supported.");
                }
            }

            try
            {
                if (namePatched || historyPatched)
                {
                    studio.UpdateBasicInfo(
                        newName ?? studio.Name,
                        historyPatched ? newHistory : studio.History
                    );
                }

                if (newCountry != null)
                {
                    studio.UpdateCountry(newCountry);
                }

                if (newFoundationDate.HasValue)
                {
                    studio.UpdateFoundationDate(newFoundationDate.Value);
                }

                if (newIsActiveState.HasValue)
                {
                    if (newIsActiveState.Value)
                    {
                        studio.Activate();
                    }
                    else
                    {
                        studio.Deactivate();
                    }
                }

                await _unitOfWork.Commit(cancellationToken);

                var response = studio.ToStudioDTO();
                return response;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred while applying patch to Studio with ID {command.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}