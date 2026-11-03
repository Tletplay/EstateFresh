using System.ComponentModel.DataAnnotations;

namespace EstateFresh.Models;

public class BookPickupViewModel
{
    [Required]
    public int ServiceID { get; set; }

    [Required]
    public int PickupSlotID { get; set; }

    [Required]
    public int DeliverySlotID { get; set; }

    public int SneakerPairs { get; set; } = 0;

    public bool GateDropoffAllowed { get; set; } = false;

    public string? SpecialInstructions { get; set; }

    // Display properties
    public List<Service>? Services { get; set; }
    public List<DeliverySlot>? PickupSlots { get; set; }
    public List<DeliverySlot>? DeliverySlots { get; set; }
}