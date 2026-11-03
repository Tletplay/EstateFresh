using EstateFresh.Data;
using EstateFresh.Models;
using EstateFresh.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstateFresh.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWeatherService _weatherService;

    public OrderController(ApplicationDbContext db, IWeatherService weatherService)
    {
        _db = db;
        _weatherService = weatherService;
    }

    // GET: Order/BookPickup  (optionally: Order/BookPickup?serviceId=2 to pre-select a service)
    [HttpGet]
    public async Task<IActionResult> BookPickup(int? serviceId)
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var viewModel = new BookPickupViewModel
        {
            ServiceID = serviceId ?? 0,
            Services = await _db.Services.ToListAsync(),
            PickupSlots = await _db.DeliverySlots
                .Where(s => s.SlotDate >= DateOnly.FromDateTime(DateTime.Today))
                .OrderBy(s => s.SlotDate).ThenBy(s => s.StartTime)
                .ToListAsync(),
            DeliverySlots = await _db.DeliverySlots
                .Where(s => s.SlotDate >= DateOnly.FromDateTime(DateTime.Today))
                .OrderBy(s => s.SlotDate).ThenBy(s => s.StartTime)
                .ToListAsync()
        };

        return View(viewModel);
    }

    // POST: Order/BookPickup
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookPickup(BookPickupViewModel model)
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            // Reload dropdowns
            model.Services = await _db.Services.ToListAsync();
            model.PickupSlots = await _db.DeliverySlots.ToListAsync();
            model.DeliverySlots = await _db.DeliverySlots.ToListAsync();
            return View(model);
        }

        var service = await _db.Services.FindAsync(model.ServiceID);
        if (service == null)
        {
            ModelState.AddModelError("", "Invalid service selected.");
            return View(model);
        }

        // Calculate deposit (fixed R50)
        var order = new Order
        {
            CustomerID = int.Parse(userId),
            ServiceID = model.ServiceID,
            PickupSlotID = model.PickupSlotID,
            DeliverySlotID = model.DeliverySlotID,
            SneakerPairs = model.SneakerPairs,
            GateDropoffAllowed = model.GateDropoffAllowed,
            SpecialInstructions = model.SpecialInstructions,
            DepositPaid = 50,
            Status = "Deposit Paid",
            CreatedAt = DateTime.UtcNow
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        // Redirect to payment
        return RedirectToAction("PayDeposit", "Payment", new { orderId = order.OrderID });
    }

    // GET: Order/MyOrders
    public async Task<IActionResult> MyOrders()
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var orders = await _db.Orders
    .Include(o => o.Service)
    .Include(o => o.Customer)
    .Where(o => o.CustomerID == int.Parse(userId))
    .OrderByDescending(o => o.CreatedAt)
    .ToListAsync();

        return View(orders);
    }

    // GET: Order/OrderDetails
    public async Task<IActionResult> OrderDetails(int orderId)
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var order = await _db.Orders
            .Include(o => o.Service)
            .Include(o => o.Customer)
            .Include(o => o.Driver)
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.CustomerID == int.Parse(userId));

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found.";
            return RedirectToAction("MyOrders");
        }

        if (!order.ActualWeightKg.HasValue)
        {
            var pickupSlot = await _db.DeliverySlots.FindAsync(order.PickupSlotID);
            if (pickupSlot != null)
            {
                ViewBag.RainWarning = await _weatherService.IsRainExpectedAsync(pickupSlot.SlotDate);
            }
        }

        return View(order);
    }

    // POST: Order/DisputeWeight
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DisputeWeight(int orderId, string disputeReason)
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var order = await _db.Orders
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.CustomerID == int.Parse(userId));

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found.";
            return RedirectToAction("MyOrders");
        }

        if (!order.ActualWeightKg.HasValue)
        {
            TempData["ErrorMessage"] = "This order doesn't have a recorded weight yet.";
            return RedirectToAction("OrderDetails", new { orderId });
        }

        if (order.WeightDisputed)
        {
            TempData["ErrorMessage"] = "This order already has a dispute under review.";
            return RedirectToAction("OrderDetails", new { orderId });
        }

        if (string.IsNullOrWhiteSpace(disputeReason))
        {
            TempData["ErrorMessage"] = "Please explain why you're disputing the weight.";
            return RedirectToAction("OrderDetails", new { orderId });
        }

        order.WeightDisputed = true;
        order.DisputeReason = disputeReason;
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Your dispute has been submitted. Our team will review it shortly.";
        return RedirectToAction("OrderDetails", new { orderId });
    }

    // GET: Order/DriverLocation
    [HttpGet]
    public async Task<IActionResult> DriverLocation(int orderId)
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false });
        }

        var order = await _db.Orders
            .Include(o => o.Driver)
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.CustomerID == int.Parse(userId));

        if (order == null || order.Status != "Out for Delivery" || order.Driver == null || !order.Driver.Latitude.HasValue)
        {
            return Json(new { success = false });
        }

        return Json(new
        {
            success = true,
            latitude = order.Driver.Latitude,
            longitude = order.Driver.Longitude,
            updatedAt = order.Driver.LocationUpdatedAt
        });
    }
}