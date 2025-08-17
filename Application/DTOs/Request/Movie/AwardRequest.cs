namespace Application.DTOs.Request.Movie
{
    public record class AwardRequest(
         int CategoryId,   
         int InstitutionId,
         int Year
    );
}
