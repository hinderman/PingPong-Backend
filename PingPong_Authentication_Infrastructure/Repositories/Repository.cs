using Microsoft.EntityFrameworkCore;
using PingPong_Authentication_Domain.Entities;
using PingPong_Authentication_Domain.Repositories;
using PingPong_Authentication_Domain.ValueObjects;
using PingPong_Authentication_Infrastructure.Persistence.Context;

namespace PingPong_Authentication_Infrastructure.Repositories
{
    internal class Repository(DataBaseContext dataBaseContext) : IRepository
    {
        private readonly DataBaseContext _dataBaseContext = dataBaseContext ?? throw new ArgumentNullException(nameof(dataBaseContext));

        public async Task Create(Users users)
        {
            using var transaction = await _dataBaseContext.BeginTransactionAsync();

            try
            {
                await _dataBaseContext.Users.AddAsync(users);
                await _dataBaseContext.SaveChangesAsync();
                await _dataBaseContext.CommitTransactionAsync(transaction);
            }
            catch
            {
                await _dataBaseContext.RollbackTransactionAsync(transaction);
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            using var transaction = await _dataBaseContext.BeginTransactionAsync();

            try
            {
                await _dataBaseContext.Users.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(u => u.State, false));
                await _dataBaseContext.SaveChangesAsync();
                await _dataBaseContext.CommitTransactionAsync(transaction);
            }
            catch
            {
                await _dataBaseContext.RollbackTransactionAsync(transaction);
                throw;
            }
        }

        public async Task<bool> ExistsId(Guid id) => await _dataBaseContext.Users.AnyAsync(x => x.Id == id);

        public async Task<bool> ExistsEmail(Email email) => await _dataBaseContext.Users.AnyAsync(x => x.Email == email);

        public async Task<bool> ExistsNickname(string nickname) => await _dataBaseContext.Users.AnyAsync(x => x.Nickname == nickname);

        public async Task<Users?> GetByEmail(Email email) => await _dataBaseContext.Users.SingleOrDefaultAsync(x => x.Email == email && x.State);

        public async Task<Users?> GetById(Guid id) => await _dataBaseContext.Users.SingleOrDefaultAsync(x => x.Id == id && x.State);

        public async Task Update(Users users)
        {
            using var transaction = await _dataBaseContext.BeginTransactionAsync();

            try
            {
                await _dataBaseContext.Users.Where(x => x.Id == users.Id).ExecuteUpdateAsync(x => x.SetProperty(u => u.Nickname, users.Nickname));
                await _dataBaseContext.SaveChangesAsync();
                await _dataBaseContext.CommitTransactionAsync(transaction);
            }
            catch
            {
                await _dataBaseContext.RollbackTransactionAsync(transaction);
                throw;
            }
        }

        public async Task SetToken(Guid id, string token)
        {
            using var transaction = await _dataBaseContext.BeginTransactionAsync();

            try
            {
                await _dataBaseContext.Users.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(u => u.Token, token));
                await _dataBaseContext.SaveChangesAsync();
                await _dataBaseContext.CommitTransactionAsync(transaction);
            }
            catch
            {
                await _dataBaseContext.RollbackTransactionAsync(transaction);
                throw;
            }
        }
    }
}
