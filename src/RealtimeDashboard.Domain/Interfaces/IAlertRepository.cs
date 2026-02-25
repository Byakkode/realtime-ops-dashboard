using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Domain.Interfaces;

public interface IAlertRepository
{
    Task<Alert?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Alert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Alert>> GetByResourceIdAsync(Guid resourceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Alert>> GetByLevelAsync(AlertLevel level, CancellationToken cancellationToken = default);
    Task AddAsync(Alert alert, CancellationToken cancellationToken = default);
    void Update(Alert alert);
}