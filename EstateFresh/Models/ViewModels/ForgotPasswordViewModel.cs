using System.ComponentModel.DataAnnotations;

namespace EstateFresh.Models;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}