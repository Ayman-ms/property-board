namespace Backend.Api.DTOs.User
{
    public class ExternalLoginDto
    {
        public string IdToken { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}