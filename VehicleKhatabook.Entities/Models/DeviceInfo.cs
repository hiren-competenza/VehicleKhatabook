using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VehicleKhatabook.Entities.Models;
using Newtonsoft.Json;

public class DeviceInfo
{
    [Key]
    public Guid DeviceInfoID { get; set; } = Guid.NewGuid();  // Primary key

    [Required]
    public Guid UserID { get; set; }  // Foreign key for User

    [MaxLength(100)]
    public string? DeviceModel { get; set; }

    [MaxLength(100)]
    public string? DeviceNumber { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public string? OS { get; set; }

    public string? AppVersion { get; set; }

    public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;  // Default value

    // Prevent circular reference when serializing to JSON
    [JsonIgnore]
    [ForeignKey("UserID")]
    public User? User { get; set; }  // Navigation property
}

