using System.Net.Http.Json;
using RealtimeDashboard.Client.Services.Models;

namespace RealtimeDashboard.Client.Services;

public class ResourceApiService
{
    private readonly HttpClient _http;

    public ResourceApiService(HttpClient http)
        => _http = http;

    public async Task<List<ResourceModel>> GetAllResourcesAsync()
    {
        var result = await _http.GetFromJsonAsync<List<ResourceModel>>("api/resources");
        return result ?? [];
    }

    public async Task<List<AlertModel>> GetActiveAlertsAsync()
    {
        var result = await _http.GetFromJsonAsync<List<AlertModel>>("api/alerts/active");
        return result ?? [];
    }

    public async Task ChangeStatusAsync(Guid resourceId, string newStatus, string changedBy)
    {
        await _http.PatchAsJsonAsync(
            $"api/resources/{resourceId}/status",
            new { newStatus, changedBy });
    }

    public async Task ResolveAlertAsync(Guid alertId, string resolvedBy)
    {
        await _http.PatchAsJsonAsync(
            $"api/alerts/{alertId}/resolve",
            new { resolvedBy });
    }
}