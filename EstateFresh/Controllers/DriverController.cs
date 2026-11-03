using EstateFresh.Data;
using EstateFresh.Models;
using EstateFresh.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstateFresh.Controllers;

public class DriverController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWeatherService _weatherService;

    public DriverController(ApplicationDbContext db, IWeatherService weatherService)
    {
        _db = db;
        _weatherService = weatherService;
    }

    // GET: Driver/Dashboard
    public async Task<IActionResult> Dashboard()
    {
        var userId = HttpContext.Session.GetString("UserID");
        var role = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(userId) || role != "Driver")
        {
            return RedirectToAction("Login", "Account");
        }

        var driverId = int.Parse(userId);

        var assignedOrders = await _db.Orders
    .Include(o => o.Service)
    .Include(o => o.Customer)
    .Where(o => o.DriverID == driverId && o.Status != "Delivered" && o.Status != "Completed")
    .OrderBy(o => o.CreatedAt)
    .ToListAsync();

        var rainWarnings = new Dictionary<int, bool>();
        foreach (var order in assignedOrders.Where(o => !o.ActualWeightKg.HasValue))
        {
            var pickupSlot = await _db.DeliverySlots.FindAsync(order.PickupSlotID);
            if (pickupSlot != null)
            {
                rainWarnings[order.OrderID] = await _weatherService.IsRainExpectedAsync(pickupSlot.SlotDate);
            }
        }
        ViewBag.RainWarnings = rainWarnings;

        return View(assignedOrders);
    }

    // GET: Driver/RecordWeight/{orderId}
    public async Task<IActionResult> RecordWeight(int orderId)
    {
        var userId = HttpContext.Session.GetString("UserID");
        var role = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(userId) || role != "Driver")
        {
            return RedirectToAction("Login", "Account");
        }

        var order = await _db.Orders
            .Include(o => o.Service)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.DriverID == int.Parse(userId));

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found or not assigned to you.";
            return RedirectToAction("Dashboard");
        }

        return View(order);
    }

    // POST: Driver/RecordWeight
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordWeight(int orderId, decimal actualWeightKg)
    {
        var userId = HttpContext.Session.GetString("UserID");
        var role = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(userId) || role != "Driver")
        {
            return RedirectToAction("Login", "Account");
        }

        var order = await _db.Orders
            .Include(o => o.Service)
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.DriverID == int.Parse(userId));

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found.";
            return RedirectToAction("Dashboard");
        }

        if (actualWeightKg <= 0 || actualWeightKg > 20)
        {
            ModelState.AddModelError("", "Weight must be between 0.1kg and 20kg.");
            return View(order);
        }

        order.ActualWeightKg = actualWeightKg;
        order.PickedUpAt = DateTime.UtcNow;

        // Calculate subtotal
        if (order.Service.RatePerKg.HasValue)
        {
            order.Subtotal = (actualWeightKg * order.Service.RatePerKg.Value) + (order.SneakerPairs * 50);
        }
        else if (order.Service.RatePerItem.HasValue)
        {
            order.Subtotal = order.SneakerPairs * 50; // Press only - items counted separately
        }

        order.BalanceDue = order.Subtotal - order.DepositPaid;
        order.Status = "Processing";

        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Weight recorded: {actualWeightKg:0.00}kg. Subtotal: R{order.Subtotal:0.00}. Balance due: R{order.BalanceDue:0.00}";
        TempData["SuccessOrderId"] = order.OrderID;
        return RedirectToAction("Dashboard");
    }

    // POST: Driver/UpdateStatus
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int orderId, string status)
    {
        var userId = HttpContext.Session.GetString("UserID");
        var role = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(userId) || role != "Driver")
        {
            return Json(new { success = false, message = "Unauthorized" });
        }

        var order = await _db.Orders
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.DriverID == int.Parse(userId));

        if (order == null)
        {
            return Json(new { success = false, message = "Order not found" });
        }

        var validStatuses = new[] { "Processing", "Out for Delivery", "Delivered" };
        if (!validStatuses.Contains(status))
        {
            return Json(new { success = false, message = "Invalid status" });
        }

        order.Status = status;

        if (status == "Delivered")
        {
            order.DeliveredAt = DateTime.UtcNow;
            // Don't mark as Completed - wait for payment
        }

        await _db.SaveChangesAsync();

        return Json(new { success = true, message = $"Status updated to {status}" });
    }

    // POST: Driver/RecordFailedPickup
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordFailedPickup(int orderId)
    {
        var userId = HttpContext.Session.GetString("UserID");
        var role = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(userId) || role != "Driver")
        {
            return RedirectToAction("Login", "Account");
        }

        var order = await _db.Orders
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.DriverID == int.Parse(userId) && o.Status == "Awaiting Pickup");

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found or not eligible.";
            return RedirectToAction("Dashboard");
        }

        order.MissedPickupAttempts += 1;
        order.LastPickupAttempt = DateTime.UtcNow;

        if (order.MissedPickupAttempts >= 2)
        {
            order.Status = "In Storage";
            order.StorageStartDate = DateTime.UtcNow;
            TempData["SuccessMessage"] = $"2nd failed attempt recorded. Order #{order.OrderID} moved to storage.";
        }
        else
        {
            TempData["SuccessMessage"] = $"Failed pickup attempt recorded for Order #{order.OrderID}. One more attempt before storage.";
        }

        await _db.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }

    // POST: Driver/UpdateLocation
    [HttpPost]
    public async Task<IActionResult> UpdateLocation(double latitude, double longitude)
    {
        var userId = HttpContext.Session.GetString("UserID");
        var role = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(userId) || role != "Driver")
        {
            return Json(new { success = false });
        }

        var driver = await _db.Users.FindAsync(int.Parse(userId));
        if (driver == null)
        {
            return Json(new { success = false });
        }

        driver.Latitude = latitude;
        driver.Longitude = longitude;
        driver.LocationUpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Json(new { success = true });
    }
}