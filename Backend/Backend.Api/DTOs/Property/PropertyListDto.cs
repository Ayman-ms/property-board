using Backend.Api.DTOs.Common;

namespace Backend.Api.DTOs.Property
{
    public class PropertyListDto : BaseDto
    {
        public int PropertyId { get; set; }
        public int UserId { get; set; }
        public int PropertyTypeId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string ListingType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        // PricePerSquareMeter is computed from Price and AreaSqm below.
        public string Currency { get; set; } = string.Empty;
        public bool IsNegotiable { get; set; }
        public string City { get; set; } = string.Empty;

        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }

        public decimal? AreaSqm { get; set; }
        public int? FloorNumber { get; set; }
        public int? TotalFloors { get; set; }
        public int? YearBuilt { get; set; }
        public string? FurnishingType { get; set; }
        public bool? HasParking { get; set; }
        public bool? HasBalcony { get; set; }
        public bool? HasGarden { get; set; }
        public bool? HasElevator { get; set; }
        public string? Street { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public int ViewsCount { get; set; }
        public int FavoritesCount { get; set; }
        public int InquiriesCount { get; set; }

        public string? MainImageUrl { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }


        // Computed
        public string FormattedPrice => Price.ToString("N0");
        public int PricePerSquareMeter =>
            AreaSqm.HasValue && AreaSqm > 0 ? (int)(Price / AreaSqm.Value)   : 0;
    }
}
