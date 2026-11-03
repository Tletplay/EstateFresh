using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateFresh.Models;

public class Payment
{
    [Key]
    public int PaymentID { get; set; }

    [ForeignKey("Order")]
    public int OrderID { get; set; }
    public virtual Order? Order { get; set; }

    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // Deposit, Balance
    public string? PayFastTransactionId { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? PaidAt { get; set; }
}