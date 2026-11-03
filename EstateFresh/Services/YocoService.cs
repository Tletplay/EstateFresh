using System.Text;
using Newtonsoft.Json;

namespace EstateFresh.Services;

public class YocoService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public YocoService(IConfiguration config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<YocoCheckoutResponse> CreateCheckoutSession(
        int orderId,
        string customerEmail,
        string customerName,
        decimal amount,
        string itemName)
    {
        var secretKey = _config["Yoco:SecretKey"];
        var apiUrl = _config["Yoco:ApiUrl"];
        var successUrl = _config["Yoco:SuccessUrl"];
        var cancelUrl = _config["Yoco:CancelUrl"];
        var failureUrl = _config["Yoco:FailureUrl"];

        // Split customer name
        var nameParts = customerName.Split(' ', 2);
        var firstName = nameParts[0];
        var lastName = nameParts.Length > 1 ? nameParts[1] : "";

        var requestBody = new
        {
            amountInCents = (int)(amount * 100),  // Yoco uses cents
            currency = "ZAR",
            successUrl = successUrl,
            cancelUrl = cancelUrl,
            failureUrl = failureUrl,
            reference = $"Order_{orderId}",
            customer = new
            {
                email = customerEmail,
                firstName = firstName,
                lastName = lastName
            },
            lineItems = new[]
            {
                new
                {
                    displayName = itemName,
                    amountInCents = (int)(amount * 100),
                    quantity = 1
                }
            }
        };

        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretKey}");

        var response = await _httpClient.PostAsync(apiUrl, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        System.Diagnostics.Debug.WriteLine($"=== YOCO CHECKOUT RESPONSE ===");
        System.Diagnostics.Debug.WriteLine($"Status: {response.StatusCode}");
        System.Diagnostics.Debug.WriteLine($"Body: {responseBody}");

        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<YocoCheckoutResponse>(responseBody);
            return result;
        }

        throw new Exception($"Yoco API error: {responseBody}");
    }
}

public class YocoCheckoutResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("redirectUrl")]
    public string RedirectUrl { get; set; }
}