using Microsoft.EntityFrameworkCore.Storage;

namespace PingPong_Authentication_Domain.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(IDbContextTransaction? iDbContextTransaction, CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(IDbContextTransaction? iDbContextTransaction, CancellationToken cancellationToken = default);
    }
}
