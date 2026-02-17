namespace Backend.Api.DTOs.Common
{
    public abstract class BaseDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}