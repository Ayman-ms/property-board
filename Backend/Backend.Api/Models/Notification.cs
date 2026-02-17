using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("notifications")]
public class Notification
{
    [Key] [Column("notification_id")] 
    public int NotificationId { get; set; }
    [Column("user_id")] 
    public int UserId { get; set; }
    [Column("type")] 
    public string Type { get; set; } = string.Empty; // مثل "NewMessage"
    [Column("title")] 
    public string Title { get; set; } = string.Empty;
    [Column("message")] 
    public string Message { get; set; } = string.Empty;
    [Column("related_id")] 
    public int? RelatedId { get; set; } // ID الخاص بالمحادثة مثلاً
    [Column("is_read")] 
    public bool IsRead { get; set; } = false;
    [Column("created_at")] 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}