using System.ComponentModel.DataAnnotations;

namespace EstateFresh.Models;

public class User
{
    [Key]
    public int UserID { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Role { get; set; } = "Customer";

    public bool IsActive { get; set; } = true;
    public bool EmailVerified { get; set; } = false;

    [MaxLength(100)]
    public string? VerificationToken { get; set; }

    [MaxLength(100)]
    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime? LocationUpdatedAt { get; set; }
}