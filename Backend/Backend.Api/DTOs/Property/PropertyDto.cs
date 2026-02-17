using Backend.Api.DTOs.Common;

namespace Backend.Api.DTOs.Property
{
    public class PropertyDto : BaseDto
    {
        public int PropertyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal AreaSqm { get; set; }
        public int? YearBuilt { get; set; }
        public int? ParkingSpaces { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasGarden { get; set; }
        public bool HasPool { get; set; }
        public bool HasElevator { get; set; }
        public bool HasBalcony { get; set; }
        public bool HasAc { get; set; }
        public int? FloorNumber { get; set; }
        public int? TotalFloors { get; set; }
        public bool IsAvailable { get; set; }
        public int ViewsCount { get; set; }
        public int UserId { get; set; }
        public int PropertyTypeId { get; set; }
        
        // Computed Properties
        public string FormattedPrice => Price.ToString("N0");
        public decimal PricePerSquareMeter => AreaSqm > 0 ? Price / AreaSqm : 0;
        public int PropertyAge => YearBuilt.HasValue ? DateTime.Now.Year - YearBuilt.Value : 0;
    }
}