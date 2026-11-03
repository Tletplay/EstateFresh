namespace EstateFresh.Models;

public class PayBalanceViewModel
{
    public int OrderID { get; set; }
    public decimal BalanceDue { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
}