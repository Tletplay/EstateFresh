namespace EstateFresh.Services;

public interface IWeatherService
{
    Task<bool> IsRainExpectedAsync(DateOnly date);
}