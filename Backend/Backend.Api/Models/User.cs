using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("first_name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column("last_name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Column("password_hash")]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("phone")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Column("profile_image")] // حسب الصورة
        [StringLength(500)]
        public string? ProfileImageUrl { get; set; }

        [Column("user_type")] // موجود في الصورة
        public string? UserType { get; set; }

        [Column("language")]
        public string? Language { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("is_verified")]
        public bool IsVerified { get; set; } = false;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        [Column("provider")]
        [StringLength(20)]
        public string? Provider { get; set; } = string.Empty;

        public string? PasswordResetToken { get; set; }
        public string? ResetTokenExpires { get; set; }
    }

}