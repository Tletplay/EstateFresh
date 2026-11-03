using EstateFresh.Models;
using Microsoft.EntityFrameworkCore;

namespace EstateFresh.Data;

public static class SeedData
{
    // Creates a test Driver account for local testing, if one doesn't already exist.
    public static async Task EnsureTestDriverAsync(ApplicationDbContext db)
    {
        const string testDriverEmail = "driver@estatefresh.com";

        bool exists = await db.Users.AnyAsync(u => u.Email == testDriverEmail);
        if (exists)
            return; // already there - don't touch it

        var driver = new User
        {
            Email = testDriverEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("driver123"),
            FullName = "Zimbali Driver",
            Phone = "0821234567",
            Role = "Driver",
            IsActive = true,
            EmailVerified = true
        };

        db.Users.Add(driver);
        await db.SaveChangesAsync();
    }

    // Creates test orders already sitting at "Processing" and "Out for Delivery",
    // so the Driver Dashboard status buttons can be tested without a full booking.
    public static async Task EnsureTestOrdersAsync(ApplicationDbContext db)
    {
        const int testCustomerId = 1;
        const int testDriverId = 13;

        bool alreadySeeded = await db.Orders
            .AnyAsync(o => o.SpecialInstructions != null && o.SpecialInstructions.StartsWith("SEEDTEST-"));
        if (alreadySeeded)
            return; // already there - don't duplicate

        // Reuse real delivery slots so nothing else breaks
        var slotIds = await db.DeliverySlots
            .OrderBy(s => s.SlotID)
            .Select(s => s.SlotID)
            .Take(2)
            .ToListAsync();

        if (slotIds.Count < 2)
            return; // slots not seeded yet - skip for now, will seed next restart

        var testOrders = new List<Order>
        {
            new Order
            {
                CustomerID = testCustomerId,
                DriverID = testDriverId,
                ServiceID = 1, // Express 24hr
                PickupSlotID = slotIds[0],
                DeliverySlotID = slotIds[0],
                ActualWeightKg = 3,
                Subtotal = 108,
                DepositPaid = 50,
                BalanceDue = 58,
                Status = "Processing",
                SpecialInstructions = "SEEDTEST-Processing",
                PickedUpAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Order
            {
                CustomerID = testCustomerId,
                DriverID = testDriverId,
                ServiceID = 1,
                PickupSlotID = slotIds[1],
                DeliverySlotID = slotIds[1],
                ActualWeightKg = 2,
                Subtotal = 72,
                DepositPaid = 50,
                BalanceDue = 22,
                Status = "Out for Delivery",
                SpecialInstructions = "SEEDTEST-OutForDelivery",
                PickedUpAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            }
        };

        db.Orders.AddRange(testOrders);
        await db.SaveChangesAsync();
    }

    // Creates a test Admin account for local testing, if one doesn't already exist.
    public static async Task EnsureTestAdminAsync(ApplicationDbContext db)
    {
        const string testAdminEmail = "admin@estatefresh.com";

        bool exists = await db.Users.AnyAsync(u => u.Email == testAdminEmail);
        if (exists)
            return;

        var admin = new User
        {
            Email = testAdminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            FullName = "Estate Fresh Admin",
            Phone = "0820000001",
            Role = "Admin",
            IsActive = true,
            EmailVerified = true
        };

        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }

    // Creates a test Manager account for local testing, if one doesn't already exist.
    public static async Task EnsureTestManagerAsync(ApplicationDbContext db)
    {
        const string testManagerEmail = "manager@estatefresh.com";

        bool exists = await db.Users.AnyAsync(u => u.Email == testManagerEmail);
        if (exists)
            return;

        var manager = new User
        {
            Email = testManagerEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
            FullName = "Estate Fresh Manager",
            Phone = "0820000002",
            Role = "Manager",
            IsActive = true,
            EmailVerified = true
        };

        db.Users.Add(manager);
        await db.SaveChangesAsync();
    }

    // Creates a test order with weight already recorded, ready for the
    // customer to dispute it, so UC12 (Weight Disputes) can be tested.
    public static async Task EnsureTestWeightDisputeOrderAsync(ApplicationDbContext db)
    {
        const int testCustomerId = 1;
        const int testDriverId = 13;
        const string marker = "SEEDTEST-WeightDispute";

        bool alreadySeeded = await db.Orders
            .AnyAsync(o => o.SpecialInstructions == marker);
        if (alreadySeeded)
            return;

        var slotId = await db.DeliverySlots
            .OrderBy(s => s.SlotID)
            .Select(s => s.SlotID)
            .FirstOrDefaultAsync();

        if (slotId == 0)
            return; // slots not seeded yet - will seed next restart

        var order = new Order
        {
            CustomerID = testCustomerId,
            DriverID = testDriverId,
            ServiceID = 1,
            PickupSlotID = slotId,
            DeliverySlotID = slotId,
            ActualWeightKg = 4,
            Subtotal = 144,
            DepositPaid = 50,
            BalanceDue = 94,
            Status = "Processing",
            SpecialInstructions = marker,
            PickedUpAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();
    }

    // Creates two test orders at "Awaiting Pickup" with no weight recorded,
    // so UC13 (missed pickup attempts) and the storage flow can be tested.
    public static async Task EnsureTestMissedPickupOrdersAsync(ApplicationDbContext db)
    {
        const int testCustomerId = 1;
        const int testDriverId = 13;

        bool alreadySeeded = await db.Orders
            .AnyAsync(o => o.SpecialInstructions == "SEEDTEST-MissedPickupA"
                        || o.SpecialInstructions == "SEEDTEST-MissedPickupB");
        if (alreadySeeded)
            return;

        var slotId = await db.DeliverySlots
            .OrderBy(s => s.SlotID)
            .Select(s => s.SlotID)
            .FirstOrDefaultAsync();

        if (slotId == 0)
            return; // slots not seeded yet - will seed next restart

        var testOrders = new List<Order>
    {
        new Order
        {
            CustomerID = testCustomerId,
            DriverID = testDriverId,
            ServiceID = 1,
            PickupSlotID = slotId,
            DeliverySlotID = slotId,
            DepositPaid = 50,
            Status = "Awaiting Pickup",
            SpecialInstructions = "SEEDTEST-MissedPickupA",
            CreatedAt = DateTime.UtcNow
        },
        new Order
        {
            CustomerID = testCustomerId,
            DriverID = testDriverId,
            ServiceID = 1,
            PickupSlotID = slotId,
            DeliverySlotID = slotId,
            DepositPaid = 50,
            Status = "Awaiting Pickup",
            SpecialInstructions = "SEEDTEST-MissedPickupB",
            CreatedAt = DateTime.UtcNow
        }
    };

        db.Orders.AddRange(testOrders);
        await db.SaveChangesAsync();
    }

    // Creates a test order already 65 days into storage, so UC14 (Donate
    // unclaimed laundry) can be tested without waiting or manual DB edits.
    public static async Task EnsureTestStorageOrderAsync(ApplicationDbContext db)
    {
        const int testCustomerId = 1;
        const int testDriverId = 13;
        const string marker = "SEEDTEST-StorageEligible";

        bool alreadySeeded = await db.Orders
            .AnyAsync(o => o.SpecialInstructions == marker);
        if (alreadySeeded)
            return;

        var slotId = await db.DeliverySlots
            .OrderBy(s => s.SlotID)
            .Select(s => s.SlotID)
            .FirstOrDefaultAsync();

        if (slotId == 0)
            return; // slots not seeded yet - will seed next restart

        var order = new Order
        {
            CustomerID = testCustomerId,
            DriverID = testDriverId,
            ServiceID = 1,
            PickupSlotID = slotId,
            DeliverySlotID = slotId,
            DepositPaid = 50,
            Status = "In Storage",
            MissedPickupAttempts = 2,
            LastPickupAttempt = DateTime.UtcNow.AddDays(-65),
            StorageStartDate = DateTime.UtcNow.AddDays(-65),
            SpecialInstructions = marker,
            CreatedAt = DateTime.UtcNow.AddDays(-66)
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();
    }

    // Creates a test order already "Delivered" 25 hours ago with an unpaid
    // balance, so the automatic balance reminder can be tested without
    // waiting a real 24 hours.
    public static async Task EnsureTestReminderOrderAsync(ApplicationDbContext db)
    {
        const int testCustomerId = 1;
        const int testDriverId = 13;
        const string marker = "SEEDTEST-BalanceReminder";

        bool alreadySeeded = await db.Orders
            .AnyAsync(o => o.SpecialInstructions == marker);
        if (alreadySeeded)
            return;

        var slotId = await db.DeliverySlots
            .OrderBy(s => s.SlotID)
            .Select(s => s.SlotID)
            .FirstOrDefaultAsync();

        if (slotId == 0)
            return;

        var order = new Order
        {
            CustomerID = testCustomerId,
            DriverID = testDriverId,
            ServiceID = 1,
            PickupSlotID = slotId,
            DeliverySlotID = slotId,
            ActualWeightKg = 3,
            Subtotal = 108,
            DepositPaid = 50,
            BalanceDue = 58,
            BalancePaid = false,
            Status = "Delivered",
            SpecialInstructions = marker,
            PickedUpAt = DateTime.UtcNow.AddDays(-2),
            DeliveredAt = DateTime.UtcNow.AddHours(-25),
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();
    }

    // Creates a test order at "Processing" (weight already recorded), ready
    // to be marked "Out for Delivery" so UC18/19 (live tracking) can be tested.
    public static async Task EnsureTestTrackingOrderAsync(ApplicationDbContext db)
    {
        const int testCustomerId = 1;
        const int testDriverId = 13;
        const string marker = "SEEDTEST-Tracking";

        bool alreadySeeded = await db.Orders
            .AnyAsync(o => o.SpecialInstructions == marker);
        if (alreadySeeded)
            return;

        var slotId = await db.DeliverySlots
            .OrderBy(s => s.SlotID)
            .Select(s => s.SlotID)
            .FirstOrDefaultAsync();

        if (slotId == 0)
            return;

        var order = new Order
        {
            CustomerID = testCustomerId,
            DriverID = testDriverId,
            ServiceID = 1,
            PickupSlotID = slotId,
            DeliverySlotID = slotId,
            ActualWeightKg = 3,
            Subtotal = 108,
            DepositPaid = 50,
            BalanceDue = 58,
            Status = "Processing",
            SpecialInstructions = marker,
            PickedUpAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();
    }

    // Second test order at "Processing", for retesting UC18/19 (live tracking)
    // without needing to reset the first one.
    public static async Task EnsureTestTrackingOrder2Async(ApplicationDbContext db)
    {
        const int testCustomerId = 1;
        const int testDriverId = 13;
        const string marker = "SEEDTEST-Tracking2";

        bool alreadySeeded = await db.Orders
            .AnyAsync(o => o.SpecialInstructions == marker);
        if (alreadySeeded)
            return;

        var slotId = await db.DeliverySlots
            .OrderBy(s => s.SlotID)
            .Select(s => s.SlotID)
            .FirstOrDefaultAsync();

        if (slotId == 0)
            return;

        var order = new Order
        {
            CustomerID = testCustomerId,
            DriverID = testDriverId,
            ServiceID = 1,
            PickupSlotID = slotId,
            DeliverySlotID = slotId,
            ActualWeightKg = 3,
            Subtotal = 108,
            DepositPaid = 50,
            BalanceDue = 58,
            Status = "Processing",
            SpecialInstructions = marker,
            PickedUpAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();
    }
}