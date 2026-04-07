using Backend.Api.DTOs.Common;

namespace Backend.Api.DTOs.User
{
    public class UserDto : BaseDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Language { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? UserType { get; set; }
    }
}