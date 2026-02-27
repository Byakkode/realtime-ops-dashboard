using MediatR;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Alerts.Queries.GetActiveAlerts;

public class GetActiveAlertsHandler : IRequestHandler<GetActiveAlertsQuery, IReadOnlyList<AlertDto>>
{
    private readonly IAlertRepository _repository;

    public GetActiveAlertsHandler(IAlertRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyList<AlertDto>> Handle(
        GetActiveAlertsQuery request,
        CancellationToken cancellationToken)
    {
        var alerts = await _repository.GetActiveAlertsAsync(cancellationToken);

        return alerts.Select(a => new AlertDto(
            a.Id, a.ResourceId, a.ResourceName,
            a.Level, a.Message, a.IsResolved,
            a.ResolvedAt, a.ResolvedBy, a.CreatedAt
        )).ToList();
    }
}