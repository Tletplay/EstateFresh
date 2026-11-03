using Microsoft.EntityFrameworkCore;
using EstateFresh.Models;

namespace EstateFresh.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<DeliverySlot> DeliverySlots { get; set; }
    public DbSet<Payment> Payments { get; set; }  // ← ADDED THIS LINE

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Seed Services (using explicit IDs to avoid conflicts)
        modelBuilder.Entity<Service>().HasData(
            new Service { ServiceID = 1, Name = "Express 24hr", RatePerKg = 36, RatePerItem = null, Description = "Pickup before 10am, delivered next day before 6pm" },
            new Service { ServiceID = 2, Name = "Eco Water-Saver", RatePerKg = 36, RatePerItem = null, Description = "Low-water industrial wash, 48hr turnaround" },
            new Service { ServiceID = 3, Name = "Premium Press Only", RatePerKg = null, RatePerItem = 12, Description = "Pickup before 12pm, returned same day pressed" }
        );

        // Seed Delivery Slots (Next 7 days)
        var slots = new List<DeliverySlot>();
        var today = DateOnly.FromDateTime(DateTime.Today);
        int slotId = 1;

        for (int day = 0; day < 7; day++)
        {
            var slotDate = today.AddDays(day);

            slots.Add(new DeliverySlot
            {
                SlotID = slotId++,
                SlotDate = slotDate,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(12, 0),
                MaxOrders = 10
            });

            slots.Add(new DeliverySlot
            {
                SlotID = slotId++,
                SlotDate = slotDate,
                StartTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(16, 0),
                MaxOrders = 10
            });

            slots.Add(new DeliverySlot
            {
                SlotID = slotId++,
                SlotDate = slotDate,
                StartTime = new TimeOnly(17, 0),
                EndTime = new TimeOnly(20, 0),
                MaxOrders = 10
            });
        }

        modelBuilder.Entity<DeliverySlot>().HasData(slots);
    }
}