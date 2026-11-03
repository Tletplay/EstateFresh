using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstateFresh.Data;
using EstateFresh.Models;

namespace EstateFresh.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: Admin/Index
    public IActionResult Index()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");
        return View();
    }

    // GET: Admin/ManageServices
    public async Task<IActionResult> ManageServices()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var services = await _db.Services.OrderBy(s => s.ServiceID).ToListAsync();
        return View(services);
    }

    // GET: Admin/CreateService
    public IActionResult CreateService()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");
        return View();
    }

    // POST: Admin/CreateService
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateService(Service model)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _db.Services.Add(model);
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Service \"{model.Name}\" created.";
        return RedirectToAction("ManageServices");
    }

    // GET: Admin/EditService/5
    public async Task<IActionResult> EditService(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var service = await _db.Services.FindAsync(id);
        if (service == null)
        {
            TempData["ErrorMessage"] = "Service not found.";
            return RedirectToAction("ManageServices");
        }

        return View(service);
    }

    // POST: Admin/EditService
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditService(Service model)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var service = await _db.Services.FindAsync(model.ServiceID);
        if (service == null)
        {
            TempData["ErrorMessage"] = "Service not found.";
            return RedirectToAction("ManageServices");
        }

        service.Name = model.Name;
        service.Description = model.Description;
        service.RatePerKg = model.RatePerKg;
        service.RatePerItem = model.RatePerItem;

        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Service \"{service.Name}\" updated.";
        return RedirectToAction("ManageServices");
    }

    // POST: Admin/DeleteService
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteService(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var service = await _db.Services.FindAsync(id);
        if (service == null)
        {
            TempData["ErrorMessage"] = "Service not found.";
            return RedirectToAction("ManageServices");
        }

        bool hasOrders = await _db.Orders.AnyAsync(o => o.ServiceID == id);
        if (hasOrders)
        {
            TempData["ErrorMessage"] = $"Cannot delete \"{service.Name}\" - it has existing orders linked to it.";
            return RedirectToAction("ManageServices");
        }

        _db.Services.Remove(service);
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Service \"{service.Name}\" deleted.";
        return RedirectToAction("ManageServices");
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("Role") == "Admin";
    }

    // GET: Admin/ManageUsers
    public async Task<IActionResult> ManageUsers(string? roleFilter)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "All")
        {
            query = query.Where(u => u.Role == roleFilter);
        }

        var users = await query.OrderBy(u => u.Role).ThenBy(u => u.FullName).ToListAsync();

        ViewBag.SelectedRole = roleFilter ?? "All";
        return View(users);
    }

    // GET: Admin/EditUser/5
    public async Task<IActionResult> EditUser(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var user = await _db.Users.FindAsync(id);
        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("ManageUsers");
        }

        return View(user);
    }

    // POST: Admin/EditUser
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(int userID, string role, bool isActive)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var currentAdminId = HttpContext.Session.GetString("UserID");
        if (currentAdminId == userID.ToString())
        {
            TempData["ErrorMessage"] = "You cannot change your own role or active status.";
            return RedirectToAction("ManageUsers");
        }

        var user = await _db.Users.FindAsync(userID);
        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("ManageUsers");
        }

        user.Role = role;
        user.IsActive = isActive;
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"{user.FullName}'s account updated.";
        return RedirectToAction("ManageUsers");
    }

    // GET: Admin/StorageOrders
    public async Task<IActionResult> StorageOrders()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var orders = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Service)
            .Where(o => o.Status == "In Storage")
            .OrderBy(o => o.StorageStartDate)
            .ToListAsync();

        return View(orders);
    }

    // POST: Admin/DonateOrder
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DonateOrder(int orderId)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var order = await _db.Orders.FindAsync(orderId);
        if (order == null || order.Status != "In Storage")
        {
            TempData["ErrorMessage"] = "Order not found or not eligible.";
            return RedirectToAction("StorageOrders");
        }

        var daysInStorage = order.StorageStartDate.HasValue
            ? (DateTime.UtcNow - order.StorageStartDate.Value).Days
            : 0;

        if (daysInStorage < 60)
        {
            TempData["ErrorMessage"] = $"Order #{order.OrderID} has only been in storage {daysInStorage} days - must reach 60 before donating.";
            return RedirectToAction("StorageOrders");
        }

        order.Donated = true;
        order.Status = "Donated";
        order.StorageFeeAccrued = daysInStorage * 20; // R20/day, locked in at time of donation
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Order #{order.OrderID} marked as donated.";
        return RedirectToAction("StorageOrders");
    }
}