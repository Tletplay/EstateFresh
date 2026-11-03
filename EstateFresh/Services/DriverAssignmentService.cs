using Microsoft.EntityFrameworkCore;
using EstateFresh.Data;

namespace EstateFresh.Services;

public class DriverAssignmentService : IDriverAssignmentService
{
    private readonly ApplicationDbContext _db;

    public DriverAssignmentService(ApplicationDbContext db)
    {
        _db = db;
    }

    // Finds any active driver with zero orders currently in progress, and assigns them.
    // Returns true if a driver was assigned, false if none were free.
    public async Task<bool> AssignDriverAsync(int orderId)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null || order.DriverID != null)
            return false; // order missing, or already has a driver

        var busyDriverIds = await _db.Orders
            .Where(o => o.DriverID != null && o.Status != "Delivered" && o.Status != "Completed")
            .Select(o => o.DriverID!.Value)
            .ToListAsync();

        var freeDriver = await _db.Users
            .Where(u => u.Role == "Driver" && u.IsActive && !busyDriverIds.Contains(u.UserID))
            .FirstOrDefaultAsync();

        if (freeDriver == null)
            return false; // no free driver right now

        order.DriverID = freeDriver.UserID;
        await _db.SaveChangesAsync();
        return true;
    }
}