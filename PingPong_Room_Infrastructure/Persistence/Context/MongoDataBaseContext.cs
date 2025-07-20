using MongoDB.Driver;
using PingPong_Room_Domain.Entities;
using PingPong_Room_Domain.Interfaces;

namespace PingPong_Room_Infrastructure.Persistence.Context
{
    internal class MongoDataBaseContext : IMongoDataBaseContext, IUnitOfWork, IAsyncDisposable
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private IClientSessionHandle? _clientSessionHandle;
        private bool _disposed = false;
        private IMongoCollection<Players>? _players;
        private IMongoCollection<Rooms>? _rooms;

        public IMongoCollection<Players> Players => _players ??= _mongoDatabase.GetCollection<Players>("Players");

        public IMongoCollection<Rooms> Rooms => _rooms ??= _mongoDatabase.GetCollection<Rooms>("Rooms");

        public bool HasActiveTransaction => _clientSessionHandle?.IsInTransaction == true;

        public bool IsDisposed => _disposed;

        public MongoDataBaseContext(IMongoClient mongoClient, string databaseName)
        {
            _mongoClient = mongoClient ?? throw new ArgumentNullException(nameof(mongoClient));

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("El nombre de la base de datos no puede ser vacio o nulo", nameof(databaseName));
            }

            _mongoDatabase = _mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre de la coleccion no puede ser vacio o nulo", nameof(name));
            }

            return _mongoDatabase.GetCollection<T>(name);
        }

        public async Task<IClientSessionHandle> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_clientSessionHandle != null)
            {
                throw new InvalidOperationException("Actualmente se encuentra una transaccion activa");
            }

            _clientSessionHandle = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            _clientSessionHandle.StartTransaction();

            return _clientSessionHandle;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_clientSessionHandle == null)
            {
                throw new InvalidOperationException("Actualmente no se encuentran transacciones disponibles");
            }

            try
            {
                await _clientSessionHandle.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                _clientSessionHandle.Dispose();
                _clientSessionHandle = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_clientSessionHandle == null)
            {
                throw new InvalidOperationException("Actualmente no se encuentran transacciones disponibles");
            }

            try
            {
                if (_clientSessionHandle.IsInTransaction)
                {
                    await _clientSessionHandle.AbortTransactionAsync(cancellationToken);
                }
            }
            finally
            {
                _clientSessionHandle.Dispose();
                _clientSessionHandle = null;
            }
        }

        public async Task<T> WithTransactionAsync<T>(Func<IClientSessionHandle, Task<T>> operation, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(operation);

            var session = await BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await operation(session);
                await CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task WithTransactionAsync(Func<IClientSessionHandle, Task> operation, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(operation);

            var session = await BeginTransactionAsync(cancellationToken);
            try
            {
                await operation(session);
                await CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(MongoDataBaseContext));
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                if (_clientSessionHandle != null)
                {
                    if (_clientSessionHandle.IsInTransaction)
                    {
                        await _clientSessionHandle.AbortTransactionAsync();
                    }
                    _clientSessionHandle.Dispose();
                    _clientSessionHandle = null;
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}

