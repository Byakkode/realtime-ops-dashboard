namespace RealtimeDashboard.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void Detach<TEntity>(TEntity entity) where TEntity : class;
}