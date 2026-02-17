using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Media;

namespace Backend.Api.DTOs.Property
{
    public class PropertyDetailsDto : BaseDto
    {
        public int PropertyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;

        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal? AreaSqm { get; set; }

        public int? YearBuilt { get; set; }

        public string Address { get; set; } = string.Empty;

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public List<PropertyMediaDto> Media { get; set; } = new();

        public string FormattedPrice => Price.ToString("N0");
    }
}
