using MediatR;

namespace RealtimeDashboard.Application.Alerts.Queries.GetActiveAlerts;

public record GetActiveAlertsQuery : IRequest<IReadOnlyList<AlertDto>>;