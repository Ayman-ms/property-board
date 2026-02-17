using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("property_media")]
    public class PropertyMedia
    {
        [Key]
        [Column("media_id")]
        public int MediaId { get; set; }

        [Column("property_id")]
        public int PropertyId { get; set; }
        [Column("media_type")]
        public string MediaType { get; set; } = "image";

        [Column("file_url")]
        public string FileUrl { get; set; } = string.Empty;
        [Column("file_name")]
        public string FileName { get; set; } = string.Empty;
        [Column("display_order")]
        public int DisplayOrder { get; set; }
        [Column("is_primary")]
        public bool IsPrimary { get; set; }

        [Column("alt_text")]
        public string AltText { get; set; } = string.Empty;
         [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public Property Property { get; set; } = null!;
    }

}