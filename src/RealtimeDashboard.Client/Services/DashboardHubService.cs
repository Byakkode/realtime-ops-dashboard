using Microsoft.AspNetCore.SignalR.Client;
using RealtimeDashboard.Client.Services.Models;

namespace RealtimeDashboard.Client.Services;

public class DashboardHubService : IAsyncDisposable
{
    private HubConnection? _connection;
    private readonly string _hubUrl;

    public event Action<ResourceStatusUpdate>? OnResourceStatusUpdated;
    public event Action<AlertModel>? OnAlertReceived;
    public event Action<string>? OnConnectionStateChanged;

    public DashboardHubService(IConfiguration configuration)
        => _hubUrl = configuration["HubUrl"] ?? "http://localhost:5039/hubs/resources";

    public bool IsConnected =>
        _connection?.State == HubConnectionState.Connected;

    public async Task StartAsync(string role = "viewer")
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _connection.On<ResourceStatusUpdate>("ReceiveResourceUpdate", update =>
        {
            OnResourceStatusUpdated?.Invoke(update);
        });

        _connection.On<AlertModel>("ReceiveAlert", alert =>
        {
            OnAlertReceived?.Invoke(alert);
        });

        _connection.Reconnecting += _ =>
        {
            OnConnectionStateChanged?.Invoke("Reconnecting...");
            return Task.CompletedTask;
        };

        _connection.Reconnected += _ =>
        {
            OnConnectionStateChanged?.Invoke("Connected");
            return Task.CompletedTask;
        };

        await _connection.StartAsync();
        await _connection.InvokeAsync("JoinRoleGroup", role);
        OnConnectionStateChanged?.Invoke("Connected");
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}

public record ResourceStatusUpdate(
    Guid ResourceId,
    string ResourceName,
    string PreviousStatus,
    string NewStatus,
    string ChangedBy,
    DateTime OccurredAt
);