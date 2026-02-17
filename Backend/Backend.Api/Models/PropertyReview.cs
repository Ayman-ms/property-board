using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("property_reviews")]
    public class PropertyReview
    {
        [Key]
        [Column("review_id")]
        public int ReviewId { get; set; }
        
        [Required]
        [Column("property_id")]
        public int PropertyId { get; set; }
        
        [Required]
        [Column("reviewer_id")]
        public int ReviewerId { get; set; }
        
        [Required]
        [Column("rating")]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Column("comment")]
        [StringLength(1000)]
        public string? Comment { get; set; }
        
        [Column("is_approved")]
        public bool IsApproved { get; set; } = false;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}