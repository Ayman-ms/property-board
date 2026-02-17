using Backend.Api.DTOs.Common;

namespace Backend.Api.DTOs.Other
{
    public class NotificationDto : BaseDto
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Message { get; set; }
        public int? RelatedId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        
        // Helper Properties
        public string TimeAgo => GetTimeAgo(CreatedAt);
        public string TypeIcon => GetTypeIcon(NotificationType);
        public string TypeColor => GetTypeColor(NotificationType);
        
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
        
        private string GetTypeIcon(string type) => type.ToLower() switch
        {
            "newmessage" => "💬",
            "propertyupdate" => "🏠",
            "favoriteadded" => "❤️",
            "reviewadded" => "⭐",
            "propertysold" => "✅",
            "propertyexpired" => "⏰",
            "savedsearchmatch" => "🔍",
            _ => "📢"
        };
        
        private string GetTypeColor(string type) => type.ToLower() switch
        {
            "newmessage" => "blue",
            "propertyupdate" => "green",
            "favoriteadded" => "red",
            "reviewadded" => "yellow",
            "propertysold" => "success",
            "propertyexpired" => "warning",
            "savedsearchmatch" => "info",
            _ => "secondary"
        };
    }
}