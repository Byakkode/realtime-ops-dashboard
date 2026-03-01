using Microsoft.EntityFrameworkCore;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public UnitOfWork(ApplicationDbContext context)
        => _context = context;
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
    
    public void Detach<TEntity>(TEntity entity) where TEntity : class
        => _context.Entry(entity).State = EntityState.Detached;
}