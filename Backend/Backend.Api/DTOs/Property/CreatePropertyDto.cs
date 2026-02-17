using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.Property
{
    public class CreatePropertyDto
{
        [Required]
        public int PropertyTypeId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        [Required]
        public string ListingType { get; set; } = "Sale"; // Sale, Rent
        public string Status { get; set; } = "Active";
        [Required]
        public decimal Price { get; set; }
        public string Currency { get; set; } = "USD";
        public bool IsNegotiable { get; set; }
        [Required]
        public string City { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal? AreaSqm { get; set; }
        public int? FloorNumber { get; set; }
        public int? TotalFloors { get; set; }
        public int? YearBuilt { get; set; }
        public bool HasParking { get; set; }
        public bool HasBalcony { get; set; }
        public bool HasGarden { get; set; }
        public bool HasElevator { get; set; }
        public string? Street { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}