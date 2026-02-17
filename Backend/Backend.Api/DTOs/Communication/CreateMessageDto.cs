using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.Communication
{
    public class CreateMessageDto
    {
        [Required(ErrorMessage = "Conversation ID is required")]
        public int ConversationId { get; set; }

        [Required(ErrorMessage = "Message content is required")]
        [StringLength(2000, ErrorMessage = "Message content cannot exceed 2000 characters")]
        public string MessageContent { get; set; } = string.Empty;
    }
}