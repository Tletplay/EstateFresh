using System.ComponentModel.DataAnnotations;

namespace EstateFresh.Models;

public class DeliverySlot
{
    [Key]
    public int SlotID { get; set; }

    public DateOnly SlotDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int MaxOrders { get; set; } = 10;
}