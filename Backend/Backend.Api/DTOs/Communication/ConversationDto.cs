using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.User;
using Backend.Api.DTOs.Property;

namespace Backend.Api.DTOs.Communication
{
    public class ConversationDto : BaseDto
    {
        public int ConversationId { get; set; }
        public int PropertyId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public bool IsActive { get; set; }
        
        // Related Data
        public PropertyListDto Property { get; set; } = new PropertyListDto();
        public UserDto Buyer { get; set; } = new UserDto();
        public UserDto Seller { get; set; } = new UserDto();
        
        // Last Message Info
        public string? LastMessageContent { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int UnreadMessagesCount { get; set; }
        
        // Helper Properties
        public bool HasUnreadMessages => UnreadMessagesCount > 0;
    }
}