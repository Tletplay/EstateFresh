using EstateFresh.Data;
using EstateFresh.Models;
using EstateFresh.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Force consistent number parsing (period as decimal separator) regardless
// of the server machine's regional settings - prevents silent 0 values
// when binding decimals/doubles from form data sent by browsers.
var defaultCulture = new System.Globalization.CultureInfo("en-US");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;
// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DbContext with SQL Server LocalDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddScoped<IDriverAssignmentService, DriverAssignmentService>();
builder.Services.AddHostedService<BalanceReminderService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Keep DeliverySlots topped up automatically, every time the app starts.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await EnsureDeliverySlotsAsync(db);
    await SeedData.EnsureTestDriverAsync(db);
    await SeedData.EnsureTestOrdersAsync(db);
    await SeedData.EnsureTestAdminAsync(db);
    await SeedData.EnsureTestManagerAsync(db);
    await SeedData.EnsureTestWeightDisputeOrderAsync(db);
    await SeedData.EnsureTestMissedPickupOrdersAsync(db);
    await SeedData.EnsureTestStorageOrderAsync(db);
    await SeedData.EnsureTestReminderOrderAsync(db);
    await SeedData.EnsureTestTrackingOrderAsync(db);
    await SeedData.EnsureTestTrackingOrder2Async(db);
}
app.Run();

static async Task EnsureDeliverySlotsAsync(ApplicationDbContext db)
{
    const int daysAhead = 14;
    var today = DateOnly.FromDateTime(DateTime.Today);

    var timeWindows = new (TimeOnly Start, TimeOnly End)[]
    {
        (new TimeOnly(9, 0), new TimeOnly(12, 0)),
        (new TimeOnly(13, 0), new TimeOnly(16, 0)),
        (new TimeOnly(17, 0), new TimeOnly(20, 0)),
    };

    // Find which dates (within our 14-day window) already have slots.
    var existingDates = await db.DeliverySlots
        .Where(s => s.SlotDate >= today && s.SlotDate < today.AddDays(daysAhead))
        .Select(s => s.SlotDate)
        .Distinct()
        .ToListAsync();

    var newSlots = new List<DeliverySlot>();

    for (int i = 0; i < daysAhead; i++)
    {
        var date = today.AddDays(i);
        if (existingDates.Contains(date))
            continue; // that day already has slots, don't duplicate

        foreach (var window in timeWindows)
        {
            newSlots.Add(new DeliverySlot
            {
                SlotDate = date,
                StartTime = window.Start,
                EndTime = window.End,
                MaxOrders = 10
            });
        }
    }

    if (newSlots.Count > 0)
    {
        db.DeliverySlots.AddRange(newSlots);
        await db.SaveChangesAsync();
    }
}