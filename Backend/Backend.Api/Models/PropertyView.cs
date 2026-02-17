using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("property_views")]
    public class PropertyView
    {
        [Key]
        [Column("view_id")]
        public int ViewId { get; set; }
        
        [Required]
        [Column("property_id")]
        public int PropertyId { get; set; }
        
        [Column("viewer_id")]
        public int? ViewerId { get; set; } // null for anonymous users
        
        [Column("ip_address")]
        [StringLength(45)]
        public string? IpAddress { get; set; }
        
        [Column("user_agent")]
        [StringLength(500)]
        public string? UserAgent { get; set; }
        
        [Column("viewed_at")]
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
        
        [Column("session_id")]
        [StringLength(100)]
        public string? SessionId { get; set; }
    }
}