using Microsoft.AspNetCore.SignalR;

namespace RealtimeDashboard.API.Hubs;

public class ResourceHub : Hub
{
    private const string AdminGroup = "Admins";
    private const string OperatorGroup = "Operators";
    private const string ViewerGroup = "Viewers";

    public async Task JoinRoleGroup(string role)
    {
        var groupName = role.ToLower() switch
        {
            "admin" => AdminGroup,
            "operator" => OperatorGroup,
            _ => ViewerGroup
        };

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Groups.AddToGroupAsync(Context.ConnectionId, "All");
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}