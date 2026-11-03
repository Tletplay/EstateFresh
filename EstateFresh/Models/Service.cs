using System.ComponentModel.DataAnnotations;

namespace EstateFresh.Models;

public class Service
{
    [Key]
    public int ServiceID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public decimal? RatePerKg { get; set; }
    public decimal? RatePerItem { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}