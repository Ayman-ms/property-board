using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.PropertyType
{
    public class CreatePropertyTypeDto
    {
        [Required(ErrorMessage = "Type name is required")]
        [StringLength(100, ErrorMessage = "Type name cannot exceed 100 characters")]
        public string TypeName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}