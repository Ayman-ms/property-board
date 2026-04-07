using Backend.Api.DTOs.Common;

namespace Backend.Api.DTOs.User
{
    public class UserProfileDto : BaseDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Language { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}";
        public string UserType { get; set; } = "User"; // Default to "User", can be "Admin", "Agent", etc.
        // Statistics
        public int PropertiesCount { get; set; }
        public int FavoritesCount { get; set; }
        public int ReviewsCount { get; set; }
    }
}