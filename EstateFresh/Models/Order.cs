using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateFresh.Models;

public class Order
{
    [Key]
    public int OrderID { get; set; }

    [ForeignKey("Customer")]
    public int CustomerID { get; set; }
    public virtual User? Customer { get; set; }

    [ForeignKey("Driver")]
    public int? DriverID { get; set; }
    public virtual User? Driver { get; set; }

    [ForeignKey("Service")]
    public int ServiceID { get; set; }
    public virtual Service? Service { get; set; }

    public int PickupSlotID { get; set; }
    public int DeliverySlotID { get; set; }

    // Weight & Pricing
    public decimal? ActualWeightKg { get; set; }
    public int SneakerPairs { get; set; } = 0;
    public decimal? Subtotal { get; set; }
    public decimal DepositPaid { get; set; } = 50;
    public decimal? BalanceDue { get; set; }
    public bool BalancePaid { get; set; } = false;
    public int BalanceReminderCount { get; set; } = 0;
    public DateTime? LastReminderSentAt { get; set; }

    // Status
    public string Status { get; set; } = "Deposit Paid";

    // Pickup tracking
    public int MissedPickupAttempts { get; set; } = 0;
    public DateTime? LastPickupAttempt { get; set; }
    public DateTime? StorageStartDate { get; set; }
    public decimal StorageFeeAccrued { get; set; } = 0;

    // Flags
    public bool GateDropoffAllowed { get; set; } = false;
    public string? SpecialInstructions { get; set; }
    public bool WeightDisputed { get; set; } = false;
    public string? DisputeReason { get; set; }
    public string? DisputeResolutionNote { get; set; }
    public bool Donated { get; set; } = false;

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PickedUpAt { get; set; }
    public DateTime? DeliveredAt { get; set; }

   
}