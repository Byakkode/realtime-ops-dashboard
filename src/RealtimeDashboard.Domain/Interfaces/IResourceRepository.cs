using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Domain.Interfaces;

public interface IResourceRepository
{
    Task<Resource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Resource>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Resource>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Resource>> GetByStatusAsync(ResourceStatus status, CancellationToken cancellationToken = default);
    Task AddAsync(Resource resource, CancellationToken cancellationToken = default);
    void Update(Resource resource);
    void Delete(Resource resource);
}