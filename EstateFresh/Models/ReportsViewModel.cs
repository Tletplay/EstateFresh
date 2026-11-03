namespace EstateFresh.Models;

public class ReportsViewModel
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<(string Status, int Count)> OrdersByStatus { get; set; } = new();
    public List<(string ServiceName, decimal Revenue, int OrderCount)> RevenueByService { get; set; } = new();
    public int PendingDisputes { get; set; }
}