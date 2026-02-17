using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("saved_searches")]
    public class SavedSearch
    {
        [Key]
        [Column("search_id")]
        public int SearchId { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Required]
        [Column("search_name")]
        [StringLength(100)]
        public string SearchName { get; set; } = string.Empty;
        
        [Column("location")]
        [StringLength(200)]
        public string? Location { get; set; }
        
        [Column("property_type_id")]
        public int? PropertyTypeId { get; set; }
        
        [Column("min_price", TypeName = "decimal(18,2)")]
        public decimal? MinPrice { get; set; }
        
        [Column("max_price", TypeName = "decimal(18,2)")]
        public decimal? MaxPrice { get; set; }
        
        [Column("min_bedrooms")]
        public int? MinBedrooms { get; set; }
        
        [Column("max_bedrooms")]
        public int? MaxBedrooms { get; set; }
        
        [Column("min_bathrooms")]
        public int? MinBathrooms { get; set; }
        
        [Column("max_bathrooms")]
        public int? MaxBathrooms { get; set; }
        
        [Column("min_area", TypeName = "decimal(10,2)")]
        public decimal? MinArea { get; set; }
        
        [Column("max_area", TypeName = "decimal(10,2)")]
        public decimal? MaxArea { get; set; }
        
        [Column("is_furnished")]
        public bool? IsFurnished { get; set; }
        
        [Column("has_garden")]
        public bool? HasGarden { get; set; }
        
        [Column("has_pool")]
        public bool? HasPool { get; set; }
        
        [Column("has_elevator")]
        public bool? HasElevator { get; set; }
        
        [Column("has_balcony")]
        public bool? HasBalcony { get; set; }
        
        [Column("has_ac")]
        public bool? HasAc { get; set; }
        
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        
        [Column("email_notifications")]
        public bool EmailNotifications { get; set; } = false;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}