using Application.Commands.Director;
using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Azure;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.SeedWork.Validation;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Linq;

namespace Application.UseCases.Directors
{
    public class PatchDirectorUseCase : ICommandHandler<PatchDirectorCommand, DirectorInfoResponse>
    {
        private readonly IRepository<Director> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public PatchDirectorUseCase(IRepository<Director> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DirectorInfoResponse> Handle(PatchDirectorCommand command, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(command.Id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {command.Id} not found.");

            // Variáveis para acumular os dados do patch, com tipos anuláveis
            string? newName = null;
            string? newBiography = null;
            Country? newCountry = null;
            Gender? newGender = null;
            DateTime? newBirthDate = null;
            bool? newIsActiveState = null;

            // Flags para controlar quais campos foram explicitamente alterados no patch
            bool namePatched = false;
            bool biographyPatched = false;
            bool genderPatched = false;
            bool birthDatePatched = false;


            foreach (var op in command.PatchDocument.Operations)
            {
                // Normaliza o caminho para comparação sem case-sensitivity
                string normalizedPath = op.path.Trim('/').ToLowerInvariant(); 

                if (op.OperationType != OperationType.Replace)
                    throw new InvalidOperationException($"Patch operation '{op.OperationType}' for path '{op.path}' is not supported.");

                switch (normalizedPath)
                {
                    case "name":
                        newName = op.value?.ToString();
                        namePatched = true;
                        break;
                    case "biography":
                        newBiography = op.value?.ToString();
                        biographyPatched = true;
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
                    case "gender":
                        // Para enums, o valor pode vir como string ou int
                        if (op.value is string genderString)
                        {
                            if (Enum.TryParse(typeof(Gender), genderString, true, out var parsedGender))
                            {
                                newGender = (Gender)parsedGender;
                                genderPatched = true;
                            }
                            else
                            {
                                throw new ValidationException("gender", $"Invalid value for 'gender': '{genderString}'. Valid values are: NotSpecified, Male, Female, Other.");
                            }
                        }
                        else if (op.value is int genderInt)
                        {
                            if (Enum.IsDefined(typeof(Gender), genderInt))
                            {
                                newGender = (Gender)genderInt;
                                genderPatched = true;
                            }
                            else
                            {
                                throw new ValidationException("gender", $"Invalid integer value for 'gender': '{genderInt}'.");
                            }
                        }
                        else
                        {
                            throw new ValidationException("gender", "Patch value for 'gender' must be a string or integer representing the enum value.");
                        }
                        break;
                    case "birthdate":
                        if (op.value is DateTime bd)
                        {
                            newBirthDate = bd;
                            birthDatePatched = true;
                        }
                        else if (op.value is string bdString && DateTime.TryParse(bdString, out var parsedBd))
                        {
                            newBirthDate = parsedBd;
                            birthDatePatched = true;
                        }
                        else
                        {
                            throw new ValidationException("birthDate", "Patch value for 'birthDate' must be a valid date.");
                        }
                        break;
                    case "isactive":
                        if (op.value is bool isActiveBool)
                            newIsActiveState = isActiveBool;
                        else
                            throw new ValidationException("isActive", "Patch value for 'isActive' must be a boolean (true/false).");
                        break;
                    default:
                        throw new InvalidOperationException($"Patch operation for path '{op.path}' is not supported.");
                }
            }

            try
            {
                // Lógica para chamar UpdateBasicInfo
                // Este método requer 'name', 'newBirthDate', 'gender' (obrigatórios) e 'biography' (opcional).
                // Devemos usar os valores do patch, ou os valores atuais do diretor se não estiverem no patch.
                if (namePatched || birthDatePatched || genderPatched || biographyPatched)
                {
                    director.UpdateBasicInfo(
                        name: newName ?? director.Name,
                        newBirthDate: newBirthDate.HasValue ? newBirthDate.Value : director.BirthDate,
                        gender: newGender.HasValue ? newGender.Value : director.Gender,
                        biography: biographyPatched ? newBiography : director.Biography
                    );
                }

                if (newCountry != null)
                    director.UpdateCountry(newCountry);

                if (newIsActiveState.HasValue)
                {
                    if (newIsActiveState.Value)
                        director.Activate();
                    else
                        director.Deactivate();
                }

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
