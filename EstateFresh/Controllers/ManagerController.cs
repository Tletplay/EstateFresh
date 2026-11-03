using EstateFresh.Data;
using EstateFresh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstateFresh.Controllers;

public class ManagerController : Controller
{
    private readonly ApplicationDbContext _db;

    public ManagerController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: Manager/Index
    public IActionResult Index()
    {
        if (!IsManager()) return RedirectToAction("Login", "Account");
        return View();
    }

    // GET: Manager/WeightDisputes
    public async Task<IActionResult> WeightDisputes()
    {
        if (!IsManager()) return RedirectToAction("Login", "Account");

        var disputes = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Service)
            .Where(o => o.WeightDisputed && o.DisputeResolutionNote == null)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();

        return View(disputes);
    }

    // GET: Manager/ResolveDispute/5
    public async Task<IActionResult> ResolveDispute(int id)
    {
        if (!IsManager()) return RedirectToAction("Login", "Account");

        var order = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Service)
            .FirstOrDefaultAsync(o => o.OrderID == id);

        if (order == null || !order.WeightDisputed)
        {
            TempData["ErrorMessage"] = "Dispute not found.";
            return RedirectToAction("WeightDisputes");
        }

        return View(order);
    }

    // POST: Manager/ResolveDispute
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolveDispute(int orderId, decimal? correctedWeight, string resolutionNote)
    {
        if (!IsManager()) return RedirectToAction("Login", "Account");

        var order = await _db.Orders
            .Include(o => o.Service)
            .FirstOrDefaultAsync(o => o.OrderID == orderId);

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found.";
            return RedirectToAction("WeightDisputes");
        }

        if (string.IsNullOrWhiteSpace(resolutionNote))
        {
            TempData["ErrorMessage"] = "Please explain your decision.";
            return RedirectToAction("ResolveDispute", new { id = orderId });
        }

        // If the manager provided a corrected weight, recalculate everything
        if (correctedWeight.HasValue && correctedWeight.Value > 0)
        {
            order.ActualWeightKg = correctedWeight.Value;

            if (order.Service.RatePerKg.HasValue)
            {
                order.Subtotal = (correctedWeight.Value * order.Service.RatePerKg.Value) + (order.SneakerPairs * 50);
            }
            else if (order.Service.RatePerItem.HasValue)
            {
                order.Subtotal = order.SneakerPairs * 50;
            }

            order.BalanceDue = order.Subtotal - order.DepositPaid;
        }
        // If no corrected weight given, the original weight is upheld as-is

        order.DisputeResolutionNote = resolutionNote;
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Dispute for Order #{order.OrderID} resolved.";
        return RedirectToAction("WeightDisputes");
    }

    private bool IsManager()
    {
        return HttpContext.Session.GetString("Role") == "Manager";
    }

    // GET: Manager/Reports
    public async Task<IActionResult> Reports()
    {
        if (!IsManager()) return RedirectToAction("Login", "Account");

        var totalOrders = await _db.Orders.CountAsync();

        var totalRevenue = await _db.Payments
            .Where(p => p.Status == "Paid")
            .SumAsync(p => p.Amount);

        var ordersByStatus = await _db.Orders
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToListAsync();

        var revenueByService = await _db.Orders
            .Include(o => o.Service)
            .Where(o => o.Subtotal.HasValue)
            .GroupBy(o => o.Service!.Name)
            .Select(g => new { ServiceName = g.Key, Revenue = g.Sum(o => o.Subtotal ?? 0), OrderCount = g.Count() })
            .OrderByDescending(g => g.Revenue)
            .ToListAsync();

        var pendingDisputes = await _db.Orders
            .CountAsync(o => o.WeightDisputed && o.DisputeResolutionNote == null);

        var model = new ReportsViewModel
        {
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            OrdersByStatus = ordersByStatus.Select(x => (x.Status, x.Count)).ToList(),
            RevenueByService = revenueByService.Select(x => (x.ServiceName, x.Revenue, x.OrderCount)).ToList(),
            PendingDisputes = pendingDisputes
        };

        return View(model);
    }
}