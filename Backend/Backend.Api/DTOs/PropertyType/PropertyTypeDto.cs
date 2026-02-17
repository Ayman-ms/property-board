using Backend.Api.DTOs.Common;

namespace Backend.Api.DTOs.PropertyType
{
    public class PropertyTypeDto : BaseDto
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PropertiesCount { get; set; }
    }
}