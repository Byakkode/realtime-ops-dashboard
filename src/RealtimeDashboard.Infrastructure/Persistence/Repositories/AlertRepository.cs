using Microsoft.EntityFrameworkCore;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Enums;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Infrastructure.Persistence.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly ApplicationDbContext _context;

    public AlertRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<Alert?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Alerts
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Alert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default)
        => await _context.Alerts
            .Where(a => !a.IsResolved)
            .OrderByDescending(a => a.Level)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Alert>> GetByResourceIdAsync(
        Guid resourceId, CancellationToken cancellationToken = default)
        => await _context.Alerts
            .Where(a => a.ResourceId == resourceId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Alert>> GetByLevelAsync(
        AlertLevel level, CancellationToken cancellationToken = default)
        => await _context.Alerts
            .Where(a => a.Level == level && !a.IsResolved)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Alert alert, CancellationToken cancellationToken = default)
        => await _context.Alerts.AddAsync(alert, cancellationToken);

    public void Update(Alert alert)
        => _context.Alerts.Update(alert);
}