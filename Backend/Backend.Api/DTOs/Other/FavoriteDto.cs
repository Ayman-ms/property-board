using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Property;
using Backend.Api.DTOs.User;

namespace Backend.Api.DTOs.Other
{
    public class FavoriteDto : BaseDto
    {
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        
        // Related Data
        public PropertyListDto Property { get; set; } = new PropertyListDto();
        public UserDto User { get; set; } = new UserDto();
    }
}