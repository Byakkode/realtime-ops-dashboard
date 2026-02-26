using Microsoft.EntityFrameworkCore;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Enums;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Infrastructure.Persistence.Repositories;

public class ResourceRepository : IResourceRepository
{
    private readonly ApplicationDbContext _context;

    public ResourceRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<Resource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Resources
            .Include(r => r.Thresholds)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Resource>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Resources
            .Include(r => r.Thresholds)
            .OrderBy(r => r.Category)
            .ThenBy(r => r.Name)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Resource>> GetByCategoryAsync(
        string category, CancellationToken cancellationToken = default)
        => await _context.Resources
            .Where(r => r.Category == category)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Resource>> GetByStatusAsync(
        ResourceStatus status, CancellationToken cancellationToken = default)
        => await _context.Resources
            .Where(r => r.Status == status)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Resource resource, CancellationToken cancellationToken = default)
        => await _context.Resources.AddAsync(resource, cancellationToken);

    public void Update(Resource resource)
        => _context.Resources.Update(resource);

    public void Delete(Resource resource)
        => _context.Resources.Remove(resource);
}