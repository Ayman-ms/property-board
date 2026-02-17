using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Models
{
  [Table("property_types")]
public class PropertyType
{
    [Key]
    [Column("type_id")]
    public int TypeId { get; set; }

    [Column("type_name")]
    public string TypeName { get; set; } = string.Empty;
    [Column("category")]
    public string Category { get; set; } = string.Empty;

    [Column("is_active")]
    public bool IsActive { get; set; }

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}

}