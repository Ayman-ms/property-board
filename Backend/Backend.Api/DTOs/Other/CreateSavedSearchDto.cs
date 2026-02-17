using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.Other
{
    public class CreateSavedSearchDto
    {
        [Required(ErrorMessage = "Search name is required")]
        [StringLength(100, ErrorMessage = "Search name cannot exceed 100 characters")]
        public string SearchName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string? Location { get; set; }

        public int? PropertyTypeId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Min price must be greater than or equal to 0")]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Max price must be greater than or equal to 0")]
        public decimal? MaxPrice { get; set; }

        [Range(0, 50, ErrorMessage = "Min bedrooms must be between 0 and 50")]
        public int? MinBedrooms { get; set; }

        [Range(0, 50, ErrorMessage = "Max bedrooms must be between 0 and 50")]
        public int? MaxBedrooms { get; set; }

        [Range(0, 20, ErrorMessage = "Min bathrooms must be between 0 and 20")]
        public int? MinBathrooms { get; set; }

        [Range(0, 20, ErrorMessage = "Max bathrooms must be between 0 and 20")]
        public int? MaxBathrooms { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Min area must be greater than or equal to 0")]
        public decimal? MinArea { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Max area must be greater than or equal to 0")]
        public decimal? MaxArea { get; set; }

        public bool? IsFurnished { get; set; }
        public bool? HasGarden { get; set; }
        public bool? HasPool { get; set; }
        public bool? HasElevator { get; set; }
        public bool? HasBalcony { get; set; }
        public bool? HasAc { get; set; }
        public bool EmailNotifications { get; set; } = false;
    }
}