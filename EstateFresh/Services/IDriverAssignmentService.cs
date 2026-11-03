namespace EstateFresh.Services;

public interface IDriverAssignmentService
{
    Task<bool> AssignDriverAsync(int orderId);
}