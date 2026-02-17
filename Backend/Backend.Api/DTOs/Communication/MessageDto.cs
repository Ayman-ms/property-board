using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.User;

namespace Backend.Api.DTOs.Communication
{
    public class MessageDto : BaseDto
    {
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string MessageContent { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }
        
        // Sender Info
        public UserDto Sender { get; set; } = new UserDto();
        
        // Helper Properties
        public string TimeAgo => GetTimeAgo(SentAt);
        
        private string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow.Subtract(dateTime);
            
            if (timeSpan <= TimeSpan.FromSeconds(60))
                return "just now";
            
            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes > 1 ? $"{timeSpan.Minutes} minutes ago" : "a minute ago";
            
            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours > 1 ? $"{timeSpan.Hours} hours ago" : "an hour ago";
            
            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days > 1 ? $"{timeSpan.Days} days ago" : "yesterday";
            
            return dateTime.ToString("MMM dd, yyyy");
        }
    }
}