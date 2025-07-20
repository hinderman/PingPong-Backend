using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PingPong_ApiGateway_Domain.Entities;
using PingPong_ApiGateway_Domain.Interfaces;

namespace PingPong_ApiGateway_Infrastructure.Persistence.Context
{
    public class DataBaseContext(DbContextOptions<DataBaseContext> dbContextOptions) : DbContext(dbContextOptions), IDataBaseContext, IUnitOfWork
    {
        private IDbContextTransaction? _contextTransaction;
        public DbSet<Users> Users { get; set; }

        public async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_contextTransaction != null)
            {
                return null;
            }

            _contextTransaction = await Database.BeginTransactionAsync(cancellationToken);
            return _contextTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction? transaction, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(transaction);

            if (transaction != _contextTransaction)
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            }

            try
            {
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(transaction, cancellationToken);
                throw;
            }
            finally
            {
                await DisposeTransactionAsync(transaction);
            }
        }

        public async Task RollbackTransactionAsync(IDbContextTransaction? transaction, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(transaction);

            if (transaction != _contextTransaction)
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            }

            try
            {
                await transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync(transaction);
            }
        }

        private async Task DisposeTransactionAsync(IDbContextTransaction? transaction)
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
                if (transaction == _contextTransaction)
                {
                    _contextTransaction = null;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);
        }
    }
}