using Backend.Api.DTOs.Common;
using Microsoft.Extensions.Configuration;

namespace Backend.Api.DTOs.Media
{
    public class PropertyMediaDto : BaseDto
    {
        public int MediaId { get; set; }
        public int PropertyId { get; set; }
        public string MediaType { get; set; } = string.Empty; // Image, Video, VirtualTour, FloorPlan
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? AltText { get; set; }
        public int DisplayOrder { get; set; }
        public long FileSize { get; set; }

        // ✅ خصائص محسوبة (لا يتم تعيينها في الـ Controller)
        public string FileUrl => BuildFileUrl();
        public string ThumbnailUrl => BuildThumbnailUrl();

        public bool IsImage => MediaType.ToLower() == "image";
        public bool IsVideo => MediaType.ToLower() == "video";

        // ✅ دعم إضافة عنوان الموقع (Domain) من إعدادات التطبيق
        private string BuildFileUrl()
        {
            string baseUrl = Environment.GetEnvironmentVariable("APP_BASE_URL") ?? string.Empty;
            return $"{baseUrl}/uploads/properties/{PropertyId}/{FileName}";
        }

        private string BuildThumbnailUrl()
        {
            string baseUrl = Environment.GetEnvironmentVariable("APP_BASE_URL") ?? string.Empty;
            return $"{baseUrl}/uploads/properties/{PropertyId}/thumbnails/{FileName}";
        }
    }
}
