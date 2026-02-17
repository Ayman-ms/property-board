using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.User
{
    public class RegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public IFormFile? ProfileImage { get; set; }
    }
}