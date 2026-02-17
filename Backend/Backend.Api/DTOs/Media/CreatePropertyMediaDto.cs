using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.Media
{
    public class CreatePropertyMediaDto
    {
        [Required(ErrorMessage = "Property ID is required")]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Media file is required")]
        public IFormFile MediaFile { get; set; } = null!;

        [Required(ErrorMessage = "Media type is required")]
        [StringLength(20, ErrorMessage = "Media type cannot exceed 20 characters")]
        public string MediaType { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Alt text cannot exceed 200 characters")]
        public string? AltText { get; set; }

        [Range(0, 100, ErrorMessage = "Display order must be between 0 and 100")]
        public int DisplayOrder { get; set; } = 0;
    }
}