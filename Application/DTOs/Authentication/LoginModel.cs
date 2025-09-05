using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Passowrd is required")]
        public string? Password { get; set; }
    }
}
