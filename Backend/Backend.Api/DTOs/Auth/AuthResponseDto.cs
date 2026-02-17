using Backend.Api.DTOs.User;

namespace Backend.Api.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
        
        // Helper Properties
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public int ExpiresInMinutes => (int)(ExpiresAt - DateTime.UtcNow).TotalMinutes;
    }
}