using MediatR;

namespace RealtimeDashboard.Application.Alerts.Commands.ResolveAlert;

public record ResolveAlertCommand(Guid AlertId, string ResolvedBy) : IRequest;