using MongoDB.Driver;

namespace PingPong_Room_Domain.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<IClientSessionHandle> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task<T> WithTransactionAsync<T>(Func<IClientSessionHandle, Task<T>> operation, CancellationToken cancellationToken = default);
        Task WithTransactionAsync(Func<IClientSessionHandle, Task> operation, CancellationToken cancellationToken = default);
    }
}