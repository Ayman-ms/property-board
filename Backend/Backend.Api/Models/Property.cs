using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("properties")]
    public class Property
    {
        [Key]
        [Column("property_id")]
        public int PropertyId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("property_type_id")]
        public int? PropertyTypeId { get; set; }

        [Column("title")]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("short_description")]
        public string? ShortDescription { get; set; }

        [Column("listing_type")] 
        public string? ListingType { get; set; }

        [Column("status")] 
        public string? Status { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("currency")]
        public string? Currency { get; set; }

        [Column("is_negotiable")]
        public bool? IsNegotiable { get; set; }

        [Column("bedrooms")]
        public int? Bedrooms { get; set; }

        [Column("bathrooms")]
        public int? Bathrooms { get; set; }

        [Column("area_sqm")]
        public decimal? AreaSqm { get; set; }

        [Column("floor_number")]
        public int? FloorNumber { get; set; }

        [Column("total_floors")]
        public int? TotalFloors { get; set; }

        [Column("year_built")]
        public int? YearBuilt { get; set; }

        [Column("has_parking")]
        public bool? HasParking { get; set; }

        [Column("has_balcony")]
        public bool? HasBalcony { get; set; }

        [Column("has_garden")]
        public bool? HasGarden { get; set; }

        [Column("has_elevator")]
        public bool? HasElevator { get; set; }

        [Column("street")]
        public string? Street { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("country")]
        public string? Country { get; set; }

        [Column("post_code")]
        public string? PostCode { get; set; }

        [Column("latitude")]
        public decimal? Latitude { get; set; }

        [Column("longitude")]
        public decimal? Longitude { get; set; }

        [Column("views_count")]
        public int? ViewsCount { get; set; }

        [Column("favorites_count")]
        public int? FavoritesCount { get; set; }

        [Column("inquiries_count")]
        public int? InquiriesCount { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("published_at")]
        public DateTime? PublishedAt { get; set; }

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        public virtual ICollection<PropertyMedia> PropertyMedia { get; set; } = new List<PropertyMedia>();
    }
}