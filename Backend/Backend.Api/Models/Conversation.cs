using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("conversations")]
    public class Conversation
    {
        [Key]
        [Column("conversation_id")]
        public int ConversationId { get; set; }

        [Column("property_id")]
        public int PropertyId { get; set; }

        [Column("buyer_user_id")]
        public int BuyerUserId { get; set; }

        [Column("seller_user_id")]
        public int SellerUserId { get; set; }

        [Column("status")]
        public string Status { get; set; } = "active";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}