using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("messages")]
public class Message
{
    [Key]
    [Column("message_id")]
    public int MessageId { get; set; }

    [Column("conversation_id")]
    public int ConversationId { get; set; }

    [Column("sender_user_id")]
    public int SenderUserId { get; set; }

    [Column("message_text")]
    public string MessageText { get; set; } = string.Empty;

    [Column("is_read")]
    public bool IsRead { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}