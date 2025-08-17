namespace Application.DTOs.Request.Movie
{
    public record class AwardRequest(
         string Name,
         string Institution,
         int Year
    );
}
