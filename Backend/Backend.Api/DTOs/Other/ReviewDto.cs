using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.User;

namespace Backend.Api.DTOs.Other
{
    public class ReviewDto : BaseDto
    {
        public int ReviewId { get; set; }
        public int PropertyId { get; set; }
        public int ReviewerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsApproved { get; set; }
        
        // Reviewer Info
        public UserDto Reviewer { get; set; } = new UserDto();
        
        // Helper Properties
        public string StarRating => new string('⭐', Rating);
        public string TimeAgo => GetTimeAgo(CreatedAt);
        
        private string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow.Subtract(dateTime);
            
            if (timeSpan <= TimeSpan.FromDays(1))
                return "today";
                            if (timeSpan <= TimeSpan.FromDays(7))
                return $"{timeSpan.Days} days ago";
            
            if (timeSpan <= TimeSpan.FromDays(30))
                return $"{timeSpan.Days / 7} weeks ago";
            
            return dateTime.ToString("MMM dd, yyyy");
        }
    }
}