using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstateFresh.Data;

namespace EstateFresh.Controllers;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;

    public DashboardController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.FullName = HttpContext.Session.GetString("FullName");
        ViewBag.Role = HttpContext.Session.GetString("Role");

        var customerId = int.Parse(userId);
        var today = DateOnly.FromDateTime(DateTime.Today);

        // Active Orders: anything not yet fully closed out
        ViewBag.ActiveOrdersCount = await _db.Orders
            .CountAsync(o => o.CustomerID == customerId
                           && o.Status != "Completed"
                           && o.Status != "Donated");

        // Next Pickup: the soonest pickup slot from today onwards for an order still awaiting collection
        var nextPickup = await _db.Orders
            .Where(o => o.CustomerID == customerId && o.Status == "Awaiting Pickup")
            .Join(_db.DeliverySlots, o => o.PickupSlotID, s => s.SlotID, (o, s) => s.SlotDate)
            .Where(d => d >= today)
            .OrderBy(d => d)
            .FirstOrDefaultAsync();

        ViewBag.NextPickupDate = nextPickup != default
            ? nextPickup.ToString("dd MMM")
            : "--";

        // Outstanding Balance: sum of unpaid balances across all orders
        ViewBag.OutstandingBalance = await _db.Orders
            .Where(o => o.CustomerID == customerId && !o.BalancePaid && o.BalanceDue.HasValue)
            .SumAsync(o => o.BalanceDue!.Value);

        // Services shown as quick-book cards on the dashboard
        ViewBag.Services = await _db.Services.ToListAsync();

        return View();
    }
}