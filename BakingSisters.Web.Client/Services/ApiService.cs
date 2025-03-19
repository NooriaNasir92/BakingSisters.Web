using System.Net.Http.Json;
using System.Text.Json;

namespace BakingSisters.Web.Client.Services;


public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        // Configure the base address from configuration
        _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7214");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<TResponse> GetAsync<TResponse>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        await EnsureSuccessStatusCodeAsync(response);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize response");
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonOptions);
        await EnsureSuccessStatusCodeAsync(response);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize response");
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
        await EnsureSuccessStatusCodeAsync(response);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize response");
    }

    public async Task DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        await EnsureSuccessStatusCodeAsync(response);
    }

    private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API request failed: {response.StatusCode} - {content}");
        }
    }
}